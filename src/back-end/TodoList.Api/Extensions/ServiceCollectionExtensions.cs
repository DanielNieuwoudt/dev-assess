﻿using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using TodoList.Api.Profiles;

namespace TodoList.Api.Extensions
{
    [ExcludeFromCodeCoverage(Justification = "Wiring")]
    public static class ServiceCollectionExtensions
    {
        private static readonly string[] Dependency = ["dependency"];

        public static IServiceCollection AddApiServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<TodoContext>(opt =>
                opt.UseInMemoryDatabase("TodoItemsDB"));
            
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(typeof(TodoItemsProfile));
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoList.Api", Version = "v1" });
            });

            return services;
        }
    }
}
