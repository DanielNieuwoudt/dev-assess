using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TodoList.Api.Common.Constants;
using TodoList.Application.Common.Errors;

namespace TodoList.Api.Common.Extensions
{
    public static class ApplicationErrorExtensions
    {
        public static BadRequestObjectResult DuplicateErrorResult(this DuplicateError duplicateError)
        {
            var badRequest = new Generated.BadRequest
            {
                Title = "The provided property is a duplicate.",
                Type = ResponseTypes.BadRequest,
                Status = StatusCodes.Status400BadRequest,
                Errors = duplicateError.errors.ToDictionary(
                    kvp => kvp.Key, 
                    kvp => kvp.Value.ToList()),
                TraceId = Activity.Current?.Id ?? string.Empty
            };

            return new BadRequestObjectResult(badRequest);
        }

        public static NotFoundObjectResult NotFoundErrorResult(this NotFoundError notFoundError)
        {
            var notFound = new Generated.NotFound
            {
                Title = "The specified resource was not found.",
                Detail = "The id provided does not exist.",
                Type = ResponseTypes.NotFound,
                Status = StatusCodes.Status404NotFound,
                TraceId = Activity.Current?.Id ?? string.Empty
            };

            return new NotFoundObjectResult(notFound);
        }
        
        public static BadRequestObjectResult ValidationErrorResult(this ValidationError validationError)
        {
            var badRequest = new Generated.BadRequest
            {
                Title = "One or more validation errors has occured.",
                Type = ResponseTypes.BadRequest,
                Status = StatusCodes.Status400BadRequest,
                Errors = validationError.errors.ToDictionary(
                    kvp => kvp.Key, 
                    kvp => kvp.Value.ToList()),
                TraceId = Activity.Current?.Id ?? string.Empty
            };

            return new BadRequestObjectResult(badRequest);
        }
    }
}
