﻿using Microsoft.AspNetCore.Mvc;
using TodoList.Api.Common.Constants;
using TodoList.Application.Common.Errors;

namespace TodoList.Api.Common.Helpers
{
    public sealed class ErrorHelper(IHttpContextAccessor httpContextAccessor, ILogger<ErrorHelper> logger) : IErrorHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        private readonly ILogger<ErrorHelper> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public BadRequestObjectResult DuplicateErrorResult(DuplicateError duplicateError)
        {
            ArgumentNullException
                .ThrowIfNull(_httpContextAccessor.HttpContext);

            var badRequest = new Generated.BadRequest
            {
                Title = ErrorMessages.ValidationError,
                Type = ResponseTypes.BadRequest,
                Status = StatusCodes.Status400BadRequest,
                Detail = ErrorMessages.PropertyDuplicate,
                Errors = duplicateError.errors.ToDictionary(
                    kvp => kvp.Key, 
                    kvp => kvp.Value.ToList()),
                TraceId = _httpContextAccessor.HttpContext.TraceIdentifier
            };

            _logger.LogWarning("Duplicate error occured: {0}", badRequest);

            return new BadRequestObjectResult(badRequest);
        }

        public NotFoundObjectResult NotFoundErrorResult(NotFoundError notFoundError)
        {
            ArgumentNullException
                .ThrowIfNull(_httpContextAccessor.HttpContext);

            var notFound = new Generated.NotFound
            {
                Title = ErrorMessages.NotFound,
                Detail = ErrorMessages.IdDoesNotExist,
                Type = ResponseTypes.NotFound,
                Status = StatusCodes.Status404NotFound,
                TraceId = _httpContextAccessor.HttpContext.TraceIdentifier
            };

            _logger.LogWarning("Not found error occured: {0}", notFound);

            return new NotFoundObjectResult(notFound);
        }
        
        public BadRequestObjectResult ValidationErrorResult(ValidationError validationError)
        {
            ArgumentNullException
                .ThrowIfNull(_httpContextAccessor.HttpContext);

            var badRequest = new Generated.BadRequest
            {
                Title = ErrorMessages.ValidationError,
                Type = ResponseTypes.BadRequest,
                Status = StatusCodes.Status400BadRequest,
                Detail = ErrorMessages.SeeErrors,
                Errors = validationError.errors.ToDictionary(
                    kvp => kvp.Key, 
                    kvp => kvp.Value.ToList()),
                TraceId = _httpContextAccessor.HttpContext.TraceIdentifier
            };

            _logger.LogWarning("Validation error occured: {0}", badRequest);

            return new BadRequestObjectResult(badRequest);
        }

        public BadRequestObjectResult IdMismatchValidationError()
        {
            ArgumentNullException
                .ThrowIfNull(_httpContextAccessor.HttpContext);

            var badRequest = new Generated.BadRequest
            {
                Title = ErrorMessages.ValidationError,
                Type = ResponseTypes.BadRequest,
                Status = StatusCodes.Status400BadRequest,
                Detail = ErrorMessages.IdMismatch,
                Errors = new Dictionary<string, List<string>>(),
                TraceId = _httpContextAccessor.HttpContext.TraceIdentifier
            };

            _logger.LogWarning("Validation error occured: {0}", badRequest);

            return new BadRequestObjectResult(badRequest);
        }
    }
}
