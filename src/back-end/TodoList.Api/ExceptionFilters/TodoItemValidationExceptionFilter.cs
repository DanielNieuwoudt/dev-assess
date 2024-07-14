using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoList.Api.Constants;
using TodoList.Application.Common.Exceptions;

namespace TodoList.Api.ExceptionFilters
{
    public class TodoItemValidationExceptionFilter : ExceptionFilterAttribute
    {
        private readonly Type _exceptionType = typeof(TodoItemValidationException);

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() != _exceptionType) return;

            var validationProblemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Type = ResponseTypes.BadRequest
            };

            if (context.Exception is TodoItemValidationException exception)
            {
                foreach (var error in exception.Errors)
                {
                    validationProblemDetails.Errors.Add(error.PropertyName, [error.ErrorMessage]);
                }
            }

            context.Result = new BadRequestObjectResult(validationProblemDetails);
            context.ExceptionHandled = true;
        }
    }
}
