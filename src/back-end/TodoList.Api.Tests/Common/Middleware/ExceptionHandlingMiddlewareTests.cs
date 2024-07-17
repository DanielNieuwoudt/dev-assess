using System.Linq.Expressions;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using TodoList.Api.Common.Constants;
using TodoList.Api.Common.Features;
using TodoList.Api.Common.Middleware;
using TodoList.Api.Generated;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TodoList.Api.Tests.Common.Middleware;

public class ExceptionHandlingMiddlewareTests
{
    private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _mockLogger = new ();
    private readonly ExceptionHandlingMiddleware _middleware;

    public ExceptionHandlingMiddlewareTests()
    {
        _middleware = new ExceptionHandlingMiddleware(_mockLogger.Object);
    }

    [Fact]
    public async Task Given_HandledException_When_InvokeAsync_Then_LogsWarning()
    {
        var exceptionContext = CreateExceptionContext(new Exception("Handled exception"));
        
        exceptionContext.ExceptionHandled = true;
        exceptionContext.HttpContext
            .Features
            .Set<IExceptionContextFeature>(new ExceptionContextFeature
            {
                ExceptionContext = exceptionContext
            });
        
        await _middleware.InvokeAsync(exceptionContext.HttpContext, _ => Task.CompletedTask);

        _mockLogger.Verify(LogsWarning, Times.Once);
    }

    [Fact]
    public async Task Given_UnhandledException_When_InvokeAsync_Then_LogsError()
    {
        var exceptionContext = CreateExceptionContext(new Exception("Unhandled exception"));
        
        exceptionContext.ExceptionHandled = false;
        exceptionContext.HttpContext
            .Features
            .Set<IExceptionContextFeature>(new ExceptionContextFeature
            {
                ExceptionContext = exceptionContext
            });
        
        await _middleware.InvokeAsync(exceptionContext.HttpContext, _ => Task.CompletedTask);

        _mockLogger.Verify(LogsError, Times.Once);
    }

    [Fact]
    public async Task Given_ExceptionThrown_When_InvokeAsync_Then_LogsErrorAndSetsResponse()
    {
        var exceptionContext = CreateExceptionContext(new Exception("Unhandled exception"));
        
        var expectedResponse = new InternalServerError
        {
            Title = "An error occurred.",
            Type = ResponseTypes.InternalServerError,
            Status = StatusCodes.Status500InternalServerError,
            Detail = "An error occurred processing your request."
        };

        await _middleware.InvokeAsync(exceptionContext.HttpContext, _ => throw new Exception("Unhandled exception"));

        _mockLogger.Verify(LogsError, Times.Once);

        exceptionContext.HttpContext.Response.ContentType
            .Should()
            .Be(MediaTypeNames.Application.Json);

        exceptionContext.HttpContext.Response.StatusCode
            .Should()
            .Be(StatusCodes.Status500InternalServerError);

        exceptionContext.HttpContext
            .Response
            .Body
            .Seek(0, SeekOrigin.Begin);
        
        var responseBody = await new StreamReader(exceptionContext.HttpContext.Response.Body)
            .ReadToEndAsync();

        var deserializedResponse = JsonSerializer
            .Deserialize<InternalServerError>(responseBody,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

        deserializedResponse
            .Should()
            .BeEquivalentTo(expectedResponse, options => 
                options.Excluding(p => p.TraceId));
    }

    private ExceptionContext CreateExceptionContext(Exception exception)
    {
        var actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext
            {
                Response =
                {
                    Body = new MemoryStream()
                }
            },
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };

        return new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = exception
        };
    }

    private Expression<Action<ILogger<ExceptionHandlingMiddleware>>> LogsWarning => logger => logger.Log(
        LogLevel.Warning,
        It.IsAny<EventId>(),
        It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Handled exception")),
        It.IsAny<Exception>(),
        ((Func<It.IsAnyType, Exception, string>)It.IsAny<object>())!);

    private Expression<Action<ILogger<ExceptionHandlingMiddleware>>> LogsError => logger => logger.Log(
        LogLevel.Error,
        It.IsAny<EventId>(),
        It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Unhandled exception")),
        It.IsAny<Exception>(),
        ((Func<It.IsAnyType, Exception, string>)It.IsAny<object>())!);

}