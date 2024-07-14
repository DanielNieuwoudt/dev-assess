using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoList.Api.Constants;

namespace TodoList.Api.ExceptionFilters
{
    public class InvalidModelStateExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var validationProblemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Type = ResponseTypes.BadRequest
                };

                context.Result = new BadRequestObjectResult(validationProblemDetails);
                context.ExceptionHandled = true;
            }
        }
    }
}
