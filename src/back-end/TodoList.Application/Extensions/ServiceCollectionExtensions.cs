using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Application.Common.Behaviours;

namespace TodoList.Application.Extensions
{
    [ExcludeFromCodeCoverage(Justification = "Wiring")]
    public static class ServiceCollectionExtensions
    {
        public static readonly string ApplicationAssembly = "TodoList.Application";

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var application = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .First(assembly => assembly.FullName!.Contains(ApplicationAssembly));

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(application);
                cfg.Lifetime = ServiceLifetime.Scoped;
            });

            services.AddValidatorsFromAssembly(typeof(ValidationBehavior<,>).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
