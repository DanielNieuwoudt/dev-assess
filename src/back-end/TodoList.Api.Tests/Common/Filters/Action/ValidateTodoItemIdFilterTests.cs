using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging.Abstractions;
using TodoList.Api.Common.Constants;
using TodoList.Api.Common.Filters.Action;
using Xunit;

namespace TodoList.Api.Tests.Common.Filters.Action
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class ValidateTodoItemIdFilterTests
    {
        private readonly NullLogger<ValidateTodoItemIdFilter> _nullLogger = new();
        
        [Fact]
        public async Task Given_ActionExecutionContext_When_IdsMatch_Then_InvokesNextDelegate()
        {
            var filter = new ValidateTodoItemIdFilter(_nullLogger);
            
            var id = Guid.NewGuid();
            
            var actionExecutingContext = CreateActionExecutingContext(new Dictionary<string, object?>
            {
                { "id", id },
                { "body", new Generated.TodoItem { Id = id } }
            });

            await filter.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult<ActionExecutedContext>(null!));

            actionExecutingContext.Result
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task Given_ActionExecutionContext_When_IdsDoNotMatch_Then_SetsResult()
        {
            var filter = new ValidateTodoItemIdFilter(_nullLogger);
            var id = Guid.NewGuid();
            var bodyId = Guid.NewGuid();

            var badRequest = new Generated.BadRequest
            {
                Title = "One or more validation errors occurred.",
                Type = ResponseTypes.BadRequest,
                Status = StatusCodes.Status400BadRequest,
                Errors = new Dictionary<string, List<string>>
                {
                    { "Id", ["The 'Id' in the url does not match the 'Id' in the body."] }
                },
                TraceId = Activity.Current?.Id ?? string.Empty
            };

            var actionExecutingContext = CreateActionExecutingContext(new Dictionary<string, object?>
            {
                { "id", id },
                { "body", new Generated.TodoItem { Id = bodyId } }
            });

            await filter.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult<ActionExecutedContext>(null!));

            actionExecutingContext.Result
                .Should()
                .BeOfType<BadRequestObjectResult>();

            var badRequestObjectResult = actionExecutingContext
                .Result as BadRequestObjectResult;
            
            badRequestObjectResult!.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);

            badRequestObjectResult!.Value
                .Should()
                .BeOfType<Generated.BadRequest>();

            badRequestObjectResult!.Value
                .Should()
                .BeEquivalentTo(badRequest);
        }

        private ActionExecutingContext CreateActionExecutingContext(Dictionary<string, object?> actionArguments)
        {
            var actionContext = new ActionContext
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            };

            var actionExecutingContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), actionArguments, null!);

            return actionExecutingContext ;
        }
    }
}
