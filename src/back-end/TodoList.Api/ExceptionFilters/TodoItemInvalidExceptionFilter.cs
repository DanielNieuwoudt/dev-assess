using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoList.Api.Constants;
using TodoList.Application.Common.Exceptions;

namespace TodoList.Api.ExceptionFilters
{
    public class TodoItemInvalidExceptionFilter : ExceptionFilterAttribute
    {
        private readonly Type _exceptionType = typeof(TodoItemInvalidException);

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() != _exceptionType) return;

            var exception = context.Exception as TodoItemInvalidException;

            var problemDetails = new ProblemDetails
            {
                Type = ResponseTypes.BadRequest,
                Title = "The provided item is not valid for the request.",
                Detail = exception!.Message
            };

            context.Result = new BadRequestObjectResult(problemDetails);

            context.ExceptionHandled = true;
        }
    }
}
