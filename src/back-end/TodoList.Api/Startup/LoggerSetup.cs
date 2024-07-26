using Serilog;

namespace TodoList.Api.Startup
{
    public static class LoggerSetup
    {
        public static IHostBuilder ConfigureSerilog(this IHostBuilder builder)
        {
            builder.UseSerilog((host, services, loggerConfig) =>
            {
                loggerConfig
                    .ReadFrom.Configuration(host.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext();
            });

            return builder;
        }
    }

}
