using DailyLife.Domain.Primitives;
using DailyLife.Infrastructure.Data.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace DailyLife.Infrastructure.BackGroundJobs;
[DisallowConcurrentExecution]
internal class PublishDomainEventJob : IJob
{
    private readonly AppIdentityDbContext _identityContext;
    private readonly ILogger<PublishDomainEventJob> _logger;
    private readonly IPublisher _publisher;
    public PublishDomainEventJob(AppIdentityDbContext identityContext, ILogger<PublishDomainEventJob> logger, IPublisher publisher)
    {
        _identityContext = identityContext;
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var outboxMessages = await _identityContext.OutboxMessages
            .Where(outboxMessage => outboxMessage.ProcessedOnUtc == null)
            .Take(10)
            .ToListAsync(context.CancellationToken);
       foreach (var outboxMessage in outboxMessages)
       {
            IDomainEvent? domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(outboxMessage.Content,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                });
            if (domainEvent is null)
            {
                _logger.LogError("Domain Event Is Null");
                continue;
            }
            await _publisher.Publish(domainEvent, context.CancellationToken);
            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;

        }
         await  _identityContext.SaveChangesAsync(context.CancellationToken);
    }
}
