using System.Text.Json;
using TodoList.Api.Common.Constants;

namespace TodoList.Api.Common.Middleware
{
    public class ExceptionHandlingMiddleware(IHostEnvironment hostEnvironment, ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
    {
        private readonly IHostEnvironment _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                // We should consider not logging sensitive information
                _logger.LogError(exception, exception.Message);
                
                switch (exception)
                {
                    default:
                        context.Response.ContentType = MediaTypes.ApplicationProblemJson;
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                        var internalServerError = new Generated.InternalServerError
                        {
                            Title = ErrorTitleMessages.InternalServerError,
                            Type = ResponseTypes.InternalServerError,
                            Status = StatusCodes.Status500InternalServerError,
                            Detail = _hostEnvironment.IsDevelopment() ? 
                                exception.Message :
                                ErrorDetailMessages.ErrorProcessingRequest,
                            TraceId = context.TraceIdentifier
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
