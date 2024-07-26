using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using TodoList.Api.Common.Helpers;
using TodoList.Api.Common.Middleware;
using TodoList.Api.Common.Mapping;

namespace TodoList.Api.Extensions
{
    [ExcludeFromCodeCoverage(Justification = "Wiring")]
    public static class ServiceCollectionExtensions
    {
        private static readonly string[] Dependency = ["dependency"];

        public static IServiceCollection AddApiServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.ConfigureAutoMapper();
            services.ConfigureControllers();
            services.ConfigureCors();
            services.ConfigureHealthChecks(configuration);
            services.ConfigureMiddleware();
            services.ConfigureServices();
            services.ConfigureSwagger();
            
            return services;
        }

        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(typeof(TodoItemMappingProfile));
            });

            return services;
        }

        public static IServiceCollection ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers();

            return services;
        }

        public static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            return services;
        }

        public static IServiceCollection ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddSqlServer(
                    connectionString: configuration.GetConnectionString("SqlServer")!,
                    healthQuery: "SELECT 1;",
                    name: "Database",
                    tags: Dependency
                )
                .AddRedis(
                    redisConnectionString: configuration.GetConnectionString("RedisCache")!,
                    name: "RedisCache",
                    tags: Dependency
                );
            
            return services;
        }
        
        public static IServiceCollection ConfigureMiddleware(this IServiceCollection services)
        {
            services.AddTransient<ExceptionHandlingMiddleware>();

            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IErrorHelper, ErrorHelper>();

            return services;
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoList.Api", Version = "v1" });
            });

            return services;
        }
    }
}
