using Microsoft.AspNetCore.Mvc;
using TodoList.Application.Common.Errors;

namespace TodoList.Api.Common.Helpers;

public interface IErrorHelper
{
    BadRequestObjectResult CreateValidationError(string message);
    BadRequestObjectResult DuplicateErrorResult(DuplicateError duplicateError);
    NotFoundObjectResult NotFoundErrorResult(NotFoundError notFoundError);
    BadRequestObjectResult ValidationErrorResult(ValidationError validationError);
}