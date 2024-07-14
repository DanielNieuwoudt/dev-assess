using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using TodoList.Api.ExceptionFilters;
using TodoList.Api.Mapping;

namespace TodoList.Api.Extensions
{
    [ExcludeFromCodeCoverage(Justification = "Wiring")]
    public static class ServiceCollectionExtensions
    {
        private static readonly string[] Dependency = ["dependency"];

        public static IServiceCollection AddApiServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(typeof(TodoItemMappingProfile));
            });

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

            services.AddControllers(options =>
            {
                options.Filters.Add(new InvalidModelStateExceptionFilter());
                options.Filters.Add(new UnhandledExceptionFilter());
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoList.Api", Version = "v1" });
            });

            return services;
        }
    }
}
