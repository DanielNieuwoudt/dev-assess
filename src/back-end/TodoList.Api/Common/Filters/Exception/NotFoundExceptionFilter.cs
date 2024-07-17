using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoList.Api.Common.Constants;
using TodoList.Api.Common.Features;
using TodoList.Application.Common.Exceptions;

namespace TodoList.Api.Common.Filters.Exception
{
    public class NotFoundExceptionFilter : ExceptionFilterAttribute
    {
        private readonly Type _exceptionType = typeof(TodoItemNotFoundException);

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() != _exceptionType) return;

            context.HttpContext.Features.Set<IExceptionContextFeature>(new ExceptionContextFeature
            {
                ExceptionContext = context
            });

            var exception = context.Exception as TodoItemNotFoundException;

            context.Result = new NotFoundObjectResult(new Generated.NotFound
            {
                Title = "The specified resource was not found.",
                Type = ResponseTypes.NotFound,
                Detail = exception!.Message,
                Status = StatusCodes.Status404NotFound,
                TraceId = Activity.Current?.Id ?? string.Empty
            });

            context.ExceptionHandled = true;
        }
    }
}
