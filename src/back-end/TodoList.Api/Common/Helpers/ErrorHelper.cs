using Microsoft.AspNetCore.Mvc;
using TodoList.Api.Common.Constants;
using TodoList.Application.Common.Errors;

namespace TodoList.Api.Common.Helpers
{
    public sealed class ErrorHelper(IHttpContextAccessor httpContextAccessor) : IErrorHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        public BadRequestObjectResult DuplicateErrorResult(DuplicateError duplicateError)
        {
            ArgumentNullException
                .ThrowIfNull(_httpContextAccessor.HttpContext);

            var badRequest = new Generated.BadRequest
            {
                Title = "One or more validation errors has occured.",
                Type = ResponseTypes.BadRequest,
                Status = StatusCodes.Status400BadRequest,
                Detail = "The provided property is a duplicate.",
                Errors = duplicateError.errors.ToDictionary(
                    kvp => kvp.Key, 
                    kvp => kvp.Value.ToList()),
                TraceId = _httpContextAccessor.HttpContext.TraceIdentifier
            };

            return new BadRequestObjectResult(badRequest);
        }

        public NotFoundObjectResult NotFoundErrorResult(NotFoundError notFoundError)
        {
            ArgumentNullException
                .ThrowIfNull(_httpContextAccessor.HttpContext);

            var notFound = new Generated.NotFound
            {
                Title = "The specified resource was not found.",
                Detail = "The 'id' provided does not exist.",
                Type = ResponseTypes.NotFound,
                Status = StatusCodes.Status404NotFound,
                TraceId = _httpContextAccessor.HttpContext.TraceIdentifier
            };

            return new NotFoundObjectResult(notFound);
        }
        
        public BadRequestObjectResult ValidationErrorResult(ValidationError validationError)
        {
            ArgumentNullException
                .ThrowIfNull(_httpContextAccessor.HttpContext);

            var badRequest = new Generated.BadRequest
            {
                Title = "One or more validation errors has occured.",
                Type = ResponseTypes.BadRequest,
                Status = StatusCodes.Status400BadRequest,
                Detail = "See the errors property for details.",
                Errors = validationError.errors.ToDictionary(
                    kvp => kvp.Key, 
                    kvp => kvp.Value.ToList()),
                TraceId = _httpContextAccessor.HttpContext.TraceIdentifier
            };

            return new BadRequestObjectResult(badRequest);
        }

        public BadRequestObjectResult CreateValidationError(string message)
        {
            ArgumentNullException
                .ThrowIfNull(_httpContextAccessor.HttpContext);

            var badRequest = new Generated.BadRequest
            {
                Title = "One or more validation errors has occured.",
                Type = ResponseTypes.BadRequest,
                Status = StatusCodes.Status400BadRequest,
                Detail = message,
                Errors = new Dictionary<string, List<string>>(),
                TraceId = _httpContextAccessor.HttpContext.TraceIdentifier
            };

            return new BadRequestObjectResult(badRequest);
        }
    }
}
