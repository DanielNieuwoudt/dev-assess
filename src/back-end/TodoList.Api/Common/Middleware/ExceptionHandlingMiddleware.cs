using System.Diagnostics;
using System.Net.Mime;
using System.Text.Json;
using TodoList.Api.Common.Constants;

namespace TodoList.Api.Common.Middleware
{
    public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                // All other exceptions are caught and logged here.
                // We may need to think of sanitisation of the exception message before logging.
                _logger.LogError(exception, exception.Message);

                context.Response.ContentType = MediaTypeNames.Application.Json;
                
                switch (exception)
                {
                    // TODO: We may want to return a more specific response for Unauthorised
                    // TODO: We may want to return a more specific response for Forbidden
                    default:
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                        var internalServerError = new Generated.InternalServerError
                        {
                            Title = ErrorTitleMessages.InternalServerError,
                            Type = ResponseTypes.InternalServerError,
                            Status = StatusCodes.Status500InternalServerError,
                            Detail = ErrorDetailMessages.ErrorProcessingRequest, // We do not return the exception message as it may contain sensitive information.
                            TraceId = Activity.Current?.Id ?? context.TraceIdentifier
                        };

                        var options = new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        };
            
                        await context
                            .Response
                            .WriteAsync(JsonSerializer.Serialize(internalServerError, options));

                        break;
                }
            }
        }
    }
}
