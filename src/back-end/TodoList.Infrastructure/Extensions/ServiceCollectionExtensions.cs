using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Application.Contracts;
using TodoList.Infrastructure.Persistence;
using TodoList.Infrastructure.Persistence.Repositories;

namespace TodoList.Infrastructure.Extensions
{
    [ExcludeFromCodeCoverage(Justification = "Wiring")]
    public static  class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TodoListDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"),
                    b => b.MigrationsAssembly(typeof(TodoListDbContext).Assembly.FullName));
            });

            services.AddScoped<ITodoItemsReadRepository, TodoItemsReadRepository>();
            services.AddScoped<ITodoItemsWriteRepository, TodoItemsWriteRepository>();

            return services;
        }
    }
}
