using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoList.Api.Constants;
using TodoList.Application.Common.Exceptions;

namespace TodoList.Api.ExceptionFilters
{
    public class TodoItemDuplicateExceptionFilter : ExceptionFilterAttribute
    {
        private readonly Type _exceptionType = typeof(TodoItemDuplicateException);

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() != _exceptionType) return;

            var exception = context.Exception as TodoItemDuplicateException;

            var problemDetails = new ProblemDetails
            {
                Type = ResponseTypes.BadRequest,
                Title = "The provided item is a duplicate.",
                Detail = exception!.Message
            };

            context.Result = new BadRequestObjectResult(problemDetails);

            context.ExceptionHandled = true;
        }
    }
}
