using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using TodoList.Api.Common.Constants;
using TodoList.Api.Generated;

namespace TodoList.Api.Common.Filters.Action
{
    public class ValidateTodoItemIdFilter(ILogger<ValidateTodoItemIdFilter> logger) : IAsyncActionFilter
    {
        private readonly ILogger<ValidateTodoItemIdFilter> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("id", out var idObj) &&
                context.ActionArguments.TryGetValue("body", out var bodyObj))
            {
                if (idObj is Guid id && bodyObj is TodoItem body)
                {
                    if (id != body.Id)
                    {
                        _logger.LogWarning("The 'Id' in the url does not match the 'Id' in the body.");

                        var badRequest = new BadRequest
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

                        context.Result = new BadRequestObjectResult(badRequest);
                        return;
                    }
                }
            }

            await next();
        }
    }
}
