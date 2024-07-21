using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using TodoList.Api.Common.Constants;
using TodoList.Api.Common.Filters.Exception;
using TodoList.Application.Common.Exceptions;
using Xunit;

namespace TodoList.Api.Tests.Common.Filters.Exception
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class NotFoundExceptionFilterTests
    {
                [Fact]
        public void Given_Exception_When_ExceptionDoesNotMatch_Then_ShouldNotSetResult()
        {
            var context = CreateExceptionContext(new System.Exception(""));
            var filter = new NotFoundExceptionFilter();
            
            filter.OnException(context);

            context.Result
                .Should()
                .BeNull();

            context.ExceptionHandled
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Given_Exception_When_ExceptionMatches_Then_ShouldSetResultAndHandle()
        {
            var notFound = new Generated.NotFound
            {
                Title = "The specified resource was not found.",
                Type = ResponseTypes.NotFound,
                Detail = "Entity \"TodoItem\" with (1) was not found.",
                Status = StatusCodes.Status404NotFound,
                TraceId = Activity.Current?.Id ?? string.Empty
            };

            var context = CreateExceptionContext(new TodoItemNotFoundException("TodoItem", "1"));
            var filter = new NotFoundExceptionFilter();
            
            filter.OnException(context);

            context.ExceptionHandled
                .Should()
                .BeTrue();

            context.Result
                .Should()
                .BeOfType<NotFoundObjectResult>();

            var notFoundObjectResult = context
                .Result as NotFoundObjectResult;
            
            notFoundObjectResult!.Value
                .Should()
                .BeOfType<Generated.NotFound>();

            notFoundObjectResult.Value
                .Should()
                .BeEquivalentTo(notFound);

            notFoundObjectResult.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }

        private ExceptionContext CreateExceptionContext(System.Exception exception)
        {
            var actionContext = new ActionContext
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            };

            return new ExceptionContext(actionContext, new List<IFilterMetadata>())
            {
                Exception = exception
            };
        }
    }
}
