using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace TodoList.Api.Startup
{
    public static class EndpointSetup
    {
        [ExcludeFromCodeCoverage(Justification = "Wiring")]
        public static WebApplication MapHealthCheckEndpoints(this WebApplication app)
        {
            app.MapHealthChecks("/health", new HealthCheckOptions { Predicate = _ => false });
            app.MapHealthChecks("/health/dependency",
                new HealthCheckOptions
                {
                    Predicate = healthCheck => healthCheck.Tags.Contains("dependency"),
                    ResponseWriter = HealthResponseWriter
                });

            return app;
        }

        public static Task HealthResponseWriter(HttpContext context, HealthReport report)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            return context.Response.WriteAsJsonAsync(new
            {
                Status = report.Status.ToString(),
                report.TotalDuration,
                Entries = report.Entries
                    .ToDictionary(
                        entry => entry.Key,
                        entry =>
                            new
                            {
                                entry.Value.Description,
                                entry.Value.Duration,
                                Status = entry.Value.Status.ToString(),
                                Error = entry.Value.Exception?.Message,
                            }
                    )
            }, options, context.RequestAborted);
        }
    }
}
