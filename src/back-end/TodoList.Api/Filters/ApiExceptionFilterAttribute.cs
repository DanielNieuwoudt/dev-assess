using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoList.Application.Common.Exceptions;

namespace TodoList.Api.Filters
{
    public sealed class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionFilterAttribute()
        {
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(TodoItemDuplicateException), HandleDuplicateException },
                { typeof(TodoItemInvalidException), HandleInvalidException },
                { typeof(TodoItemNotFoundException), HandleNotFoundException },
                { typeof(TodoItemValidationException), HandleValidationException }
            };
        }
        
        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            //TODO: Centrally log exception here

            var type = context.Exception.GetType();
            if (_exceptionHandlers.TryGetValue(type, out var handler))
            {
                handler.Invoke(context);
                return;
            }

            if (!context.ModelState.IsValid)
            {
                HandleInvalidModelStateException(context);
                return;
            }

            HandleUnknownException(context);
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "An error occurred while processing your request.",
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }

        private void HandleInvalidModelStateException(ExceptionContext context)
        {
            var validationProblemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            context.Result = new BadRequestObjectResult(validationProblemDetails);
            context.ExceptionHandled = true;
        }

        private void HandleValidationException(ExceptionContext context)
        {
            var validationProblemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
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

        private void HandleDuplicateException(ExceptionContext context)
        {
            var exception = context.Exception as TodoItemDuplicateException;

            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The provided item is a duplicate.",
                Detail = exception!.Message
            };

            context.Result = new UnprocessableEntityObjectResult(problemDetails);

            context.ExceptionHandled = true;
        }

        private void HandleInvalidException(ExceptionContext context)
        {
            var exception = context.Exception as TodoItemInvalidException;

            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The provided item is not valid for the request.",
                Detail = exception!.Message
            };

            context.Result = new UnprocessableEntityObjectResult(problemDetails);

            context.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as TodoItemNotFoundException;

            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The specified resource was not found.",
                Detail = exception!.Message
            };

            context.Result = new NotFoundObjectResult(problemDetails);

            context.ExceptionHandled = true;
        }
    }
}
