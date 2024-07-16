using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using TodoList.Api.Common.Features;
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

                // We use the exception feature to log exceptions that are handled by the exception filters.
                var exceptionFeature = context.Features.Get<IExceptionContextFeature>();
                if (exceptionFeature is { ExceptionContext: not null })
                {
                    if (exceptionFeature.ExceptionContext.ExceptionHandled)
                    {
                        logger.LogWarning(exceptionFeature.ExceptionContext.Exception, exceptionFeature.ExceptionContext.Exception.Message);
                    }
                    else
                    {
                        logger.LogError(exceptionFeature.ExceptionContext.Exception, exceptionFeature.ExceptionContext.Exception.Message);
                    }
                }
            }
            catch (Exception exception)
            {
                // All other exceptions are caught and logged here.
                _logger.LogError(exception, exception.Message);

                context.Response.ContentType = MediaTypeNames.Application.Json;
                
                switch (exception)
                {
                    // TODO: We may want to return a more specific response for Unauthorised
                    // TODO: We may want to return a more specific response for Forbidden
                    default:
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var internalServerError = new Generated.InternalServerError
                        {
                            Title = "An error occurred.",
                            Type = ResponseTypes.InternalServerError,
                            Status = (int)HttpStatusCode.InternalServerError,
                            Detail = "An error occurred processing your request.", // We do not return the exception message as it may contain sensitive information.
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
