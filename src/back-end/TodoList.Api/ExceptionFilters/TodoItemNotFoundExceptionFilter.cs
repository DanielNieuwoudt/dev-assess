using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoList.Api.Constants;
using TodoList.Application.Common.Exceptions;

namespace TodoList.Api.ExceptionFilters
{
    public class TodoItemNotFoundExceptionFilter : ExceptionFilterAttribute
    {
        private readonly Type _exceptionType = typeof(TodoItemNotFoundException);

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() != _exceptionType) return;

            var exception = context.Exception as TodoItemNotFoundException;

            var problemDetails = new ProblemDetails
            {
                Type = ResponseTypes.NotFound,
                Title = "The specified resource was not found.",
                Detail = exception!.Message
            };

            context.Result = new NotFoundObjectResult(problemDetails);

            context.ExceptionHandled = true;
        }
    }
}
