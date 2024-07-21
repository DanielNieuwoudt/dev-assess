using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using FluentValidation.Results;
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
    public class DuplicateExceptionFilterTests
    {
        [Fact]
        public void Given_Exception_When_ExceptionDoesNotMatch_Then_ShouldNotSetResult()
        {
            var context = CreateExceptionContext(new System.Exception(""));
            var filter = new DuplicateExceptionFilter();
            
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
                Title = "The provided property is a duplicate.",
                Type = ResponseTypes.BadRequest,
                Status = StatusCodes.Status400BadRequest,
                Errors = new Dictionary<string, List<string>>
                {
                    { "Id", ["Should not be empty."] }
                },
                TraceId = Activity.Current?.Id ?? string.Empty
            };

            var context = CreateExceptionContext(new TodoItemDuplicateException(validationFailures));
            var filter = new DuplicateExceptionFilter();
            
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
                .Be(StatusCodes.Status400BadRequest);
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
