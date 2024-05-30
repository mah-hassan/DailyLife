using DailyLife.Domain.Repositories;
using DailyLife.Infrastructure.Data.Identity;
using DailyLife.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DailyLife.Infrastructure.Interceptors;
using Quartz;
using DailyLife.Infrastructure.BackGroundJobs;
using DailyLife.Infrastructure.Services;
using DailyLife.Application.Abstractions;
using DailyLife.Infrastructure.Authentication;
using DailyLife.Infrastructure.Data.Business;
using DailyLife.Infrastructure.Repositories.Caching;

namespace DailyLife.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddDbContext<AppIdentityDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();

            options.UseSqlServer(configuration.GetConnectionString("DailyLife.Identity"))
                .AddInterceptors(interceptor);
        });
        services.AddDbContext<BusinessDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();

            options.UseSqlServer(configuration.GetConnectionString("DailyLife"))
            .AddInterceptors(interceptor);
        });

        services.AddQuartz(configuration =>
        {
            var jobKey = new JobKey(nameof(PublishDomainEventJob));
            configuration.AddJob<PublishDomainEventJob>(jobKey)
            .AddTrigger(trigger =>
                trigger.ForJob(jobKey)
                       .WithSimpleSchedule(schedule => 
                            schedule.WithIntervalInSeconds(15)
                                    .RepeatForever()));
            configuration.UseMicrosoftDependencyInjectionJobFactory();
        });
        services.AddQuartzHostedService();
        services.AddMediatR(c => c.RegisterServicesFromAssembly(AssemblyReference.Assembly));
        #region scoped services
        services.AddScoped<IFavoritesRepository, FavoritesRepository>();
        services.AddScoped<IApplicationDbContext, BusinessDbContext>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();  
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IAvatarProvider, AvatarProvider>();
        services.AddScoped<IBusinessRepository, CachedBusinessRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<CategoryRepository>();
        services.AddScoped<BusinessRepository>();
        services.AddScoped<ICategoryRepository, CachedCategoryRepository>();
        #endregion
        #region transient services
        services.AddTransient<IEmailService, EmailService>();
        #endregion
        #region Singleton
        services.AddSingleton<IJwtProvider, JwtProvider>();
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        #endregion
        return services;
    }
}