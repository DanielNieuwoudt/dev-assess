using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using TodoList.Api.Constants;
using TodoList.Application.Common.Exceptions;

namespace TodoList.Api.Common.ExceptionFilters
{
    public class TodoItemNotFoundExceptionFilter : ExceptionFilterAttribute
    {
        private readonly Type _exceptionType = typeof(TodoItemNotFoundException);

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() != _exceptionType) return;

            var exception = context.Exception as TodoItemNotFoundException;

            context.Result = new NotFoundObjectResult(new Generated.NotFound
            {
                Title = "The specified resource was not found.",
                Type = ResponseTypes.NotFound,
                Detail = exception!.Message,
                Status = (int)System.Net.HttpStatusCode.NotFound,
                TraceId = Activity.Current?.Id ?? string.Empty
            });

            context.ExceptionHandled = true;
        }
    }
}
