using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using TodoList.Api.Common.ExceptionFilters;
using TodoList.Api.Constants;
using TodoList.Application.Common.Exceptions;
using Xunit;

namespace TodoList.Api.Tests.Common.ExceptionFilters
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class TodoItemNotFoundExceptionFilterTests
    {
                [Fact]
        public void Given_Exception_When_ExceptionDoesNotMatch_Then_ShouldNotSetResult()
        {
            var context = CreateExceptionContext(new Exception(""));
            var filter = new TodoItemNotFoundExceptionFilter();
            
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
                Status = (int)HttpStatusCode.NotFound,
                TraceId = Activity.Current?.Id ?? string.Empty
            };

            var context = CreateExceptionContext(new TodoItemNotFoundException("TodoItem", "1"));
            var filter = new TodoItemNotFoundExceptionFilter();
            
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
                .Be((int)HttpStatusCode.NotFound);
        }

        private ExceptionContext CreateExceptionContext(Exception exception)
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
