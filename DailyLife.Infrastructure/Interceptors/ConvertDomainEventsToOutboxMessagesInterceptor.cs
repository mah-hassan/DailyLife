using DailyLife.Domain.Entities;
using DailyLife.Domain.Primitives;
using DailyLife.Infrastructure.Data.Business;
using DailyLife.Infrastructure.Data.Identity;
using DailyLife.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace DailyLife.Infrastructure.Interceptors;

public sealed class ConvertDomainEventsToOutboxMessagesInterceptor
    : SaveChangesInterceptor
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ConvertDomainEventsToOutboxMessagesInterceptor(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        var messages = new List<OutboxMessage>();
        if (dbContext is AppIdentityDbContext context)
        {

            var user = dbContext.ChangeTracker.Entries<AppUser>()
                .Select(e => e.Entity).FirstOrDefault();
           
             messages = dbContext.ChangeTracker
            .Entries<AppUser>()
            .Select(e => e.Entity)
            .SelectMany(user =>
            {
                var domainEnents = user.GetEvents();
                user.ClearDomainEvents();
                return domainEnents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = domainEvent.GetType().Name,
                OccurredOnUtc = DateTime.UtcNow,
                Content = JsonConvert.SerializeObject(domainEvent,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                })
            }).ToList();
            dbContext.Set<OutboxMessage>().AddRange(messages);
        }
        else
        {
            using var scope = _scopeFactory.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();

            messages = dbContext.ChangeTracker
                .Entries<AggregateRoot>()
                .Select(e => e.Entity)
                .SelectMany(aggregateRoot =>
                {
                    var domainEnents = aggregateRoot.Events;
                    aggregateRoot.ClearDomainEvents();
                    return domainEnents;
                }).Select(domainEvent => new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    Type = domainEvent.GetType().Name,
                    OccurredOnUtc = DateTime.UtcNow,
                    Content = JsonConvert.SerializeObject(domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                    })
                }).ToList();

            context.Set<OutboxMessage>().AddRange(messages);
            context.SaveChangesAsync(cancellationToken);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

}
