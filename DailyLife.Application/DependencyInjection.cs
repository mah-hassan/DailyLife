using DailyLife.Application.Abstractions.Behaviors;
using DailyLife.Application.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace DailyLife.Application
{
    public static class DependencyInjection 
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assemblyReference = typeof(DependencyInjection).Assembly;
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(assemblyReference);
                configuration.AddOpenBehavior(typeof(LoggingPipeLineBehavior<,>));
            });
            services.AddSingleton<Mapper>();
            return services;    
        }
    }
}