using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoList.Api.Constants;

namespace TodoList.Api.ExceptionFilters
{
    public class UnhandledExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "An error occurred while processing your request.",
                Status = StatusCodes.Status500InternalServerError,
                Type = ResponseTypes.InternalServerError
            };

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}
