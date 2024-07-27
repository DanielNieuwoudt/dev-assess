using Microsoft.AspNetCore.Mvc;
using TodoList.Application.Common.Errors;

namespace TodoList.Api.Common.Helpers;

public interface IErrorHelper
{
    BadRequestObjectResult DuplicateErrorResult(ApplicationError applicationError);
    BadRequestObjectResult IdMismatchValidationError();
    NotFoundObjectResult NotFoundErrorResult(ApplicationError applicationError);
    BadRequestObjectResult ValidationErrorResult(ApplicationError applicationError);
}