using Serilog;
using System.Diagnostics.CodeAnalysis;
using TodoList.Api.Common.Middleware;
using TodoList.Api.Startup;

namespace TodoList.Api.Extensions
{
    [ExcludeFromCodeCoverage(Justification = "Wiring")]
    public static class WebApplicationExtensions
    {
        public static void ConfigureApi(this WebApplication app, IConfiguration configuration, IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoList.Api v1"));
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("AllowAllHeaders");

            
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHealthCheckEndpoints();
        }
    }
}
