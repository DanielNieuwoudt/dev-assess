using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using TodoList.Api.Common.ExceptionFilters;
using TodoList.Api.Common.Constants;
using TodoList.Application.Common.Exceptions;
using Xunit;

namespace TodoList.Api.Tests.Common.ExceptionFilters
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class TodoItemValidationExceptionFilterTests
    {
               [Fact]
        public void Given_Exception_When_ExceptionDoesNotMatch_Then_ShouldNotSetResult()
        {
            var context = CreateExceptionContext(new Exception(""));
            var filter = new TodoItemValidationExceptionFilter();
            
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
            var validationFailures = new List<ValidationFailure>
            {
                new ("Id", "Should not be empty.")
            };

            var badRequest = new Generated.BadRequest
            {
                Title = "One or more validation errors occurred.",
                Type = ResponseTypes.BadRequest,
                Status = (int)HttpStatusCode.BadRequest,
                Errors = new Dictionary<string, List<string>>
                {
                    { "Id", ["Should not be empty."] }
                },
                TraceId = Activity.Current?.Id ?? string.Empty
            };

            var context = CreateExceptionContext(new TodoItemValidationException(validationFailures));
            var filter = new TodoItemValidationExceptionFilter();
            
            filter.OnException(context);

            context.ExceptionHandled
                .Should()
                .BeTrue();

            context.Result
                .Should()
                .BeOfType<BadRequestObjectResult>();

            var badRequestObjectResult = context
                .Result as BadRequestObjectResult;
            
            badRequestObjectResult!.Value
                .Should()
                .BeOfType<Generated.BadRequest>();

            badRequestObjectResult.Value
                .Should()
                .BeEquivalentTo(badRequest);

            badRequestObjectResult.StatusCode
                .Should()
                .Be((int)HttpStatusCode.BadRequest);
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
