using Microsoft.AspNetCore.Mvc;
using TodoList.Api.Common.Constants;
using TodoList.Application.Common.Enumerations;
using TodoList.Application.Common.Errors;

namespace TodoList.Api.Common.Helpers
{
    public sealed class ErrorHelper(IHttpContextAccessor httpContextAccessor, ILogger<ErrorHelper> logger) : IErrorHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        private readonly ILogger<ErrorHelper> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public BadRequestObjectResult DuplicateErrorResult(ApplicationError applicationError)
        {
            ArgumentNullException
                .ThrowIfNull(applicationError);

            ArgumentNullException
                .ThrowIfNull(_httpContextAccessor.HttpContext);

            _httpContextAccessor.HttpContext.Response
                .ContentType = MediaTypes.ApplicationProblemJson;

            var badRequest = new Generated.BadRequest
            {
                Title = ErrorTitleMessages.ValidationError,
                Type = ResponseTypes.BadRequest,
                Status = StatusCodes.Status400BadRequest,
                Detail = ErrorDetailMessages.PropertyDuplicate,
                Errors = applicationError.Errors.ToDictionary(
                    kvp => kvp.Key, 
                    kvp => kvp.Value.ToList()),
                TraceId = _httpContextAccessor.HttpContext!.TraceIdentifier
            };

            _logger.LogWarning("An error occured for the request. Reason: {Reason} TraceId: {TraceId}",
                applicationError.Reason, badRequest.TraceId);

            return new BadRequestObjectResult(badRequest);
        }

        public NotFoundObjectResult NotFoundErrorResult(ApplicationError applicationError)
        {
            ArgumentNullException
                .ThrowIfNull(applicationError);

            ArgumentNullException
                .ThrowIfNull(_httpContextAccessor.HttpContext);

            _httpContextAccessor.HttpContext.Response
                .ContentType = MediaTypes.ApplicationProblemJson;

            var notFound = new Generated.NotFound
            {
                Title = ErrorTitleMessages.NotFound,
                Detail = ErrorDetailMessages.IdDoesNotExist,
                Type = ResponseTypes.NotFound,
                Status = StatusCodes.Status404NotFound,
                TraceId = _httpContextAccessor.HttpContext.TraceIdentifier
            };

            _logger.LogWarning("An error occured for the request. Reason: {Reason} TraceId: {TraceId}",
                applicationError.Reason, notFound.TraceId);

            return new NotFoundObjectResult(notFound);
        }
        
        public BadRequestObjectResult ValidationErrorResult(ApplicationError applicationError)
        {
            ArgumentNullException
                .ThrowIfNull(applicationError);
            
            ArgumentNullException
                .ThrowIfNull(_httpContextAccessor.HttpContext);

            _httpContextAccessor.HttpContext.Response
                .ContentType = MediaTypes.ApplicationProblemJson;

            var badRequest = new Generated.BadRequest
            {
                Title = ErrorTitleMessages.ValidationError,
                Type = ResponseTypes.BadRequest,
                Status = StatusCodes.Status400BadRequest,
                Detail = ErrorDetailMessages.SeeErrors,
                Errors = applicationError.Errors.ToDictionary(
                    kvp => kvp.Key, 
                    kvp => kvp.Value.ToList()),
                TraceId = _httpContextAccessor.HttpContext!.TraceIdentifier
            };

            _logger.LogWarning("An error occured for the request. Reason: {Reason} TraceId: {TraceId}",
                applicationError.Reason, badRequest.TraceId);

            return new BadRequestObjectResult(badRequest);
        }

        public BadRequestObjectResult IdMismatchValidationError()
        {
            ArgumentNullException
                .ThrowIfNull(_httpContextAccessor.HttpContext);

            _httpContextAccessor.HttpContext.Response
                .ContentType = MediaTypes.ApplicationProblemJson;

            var badRequest = new Generated.BadRequest
            {
                Title = ErrorTitleMessages.ValidationError,
                Type = ResponseTypes.BadRequest,
                Status = StatusCodes.Status400BadRequest,
                Detail = ErrorDetailMessages.IdMismatch,
                Errors = new Dictionary<string, List<string>>(),
                TraceId = _httpContextAccessor.HttpContext!.TraceIdentifier
            };

            _logger.LogWarning("An error occured for the request. Reason: {Reason} TraceId: {TraceId}",
                ErrorReason.Validation, badRequest.TraceId);

            return new BadRequestObjectResult(badRequest);
        }
    }
}
