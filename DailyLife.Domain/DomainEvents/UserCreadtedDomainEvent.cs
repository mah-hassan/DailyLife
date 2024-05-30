using DailyLife.Domain.Primitives;

namespace DailyLife.Domain.DomainEvents;

public record UserCreadtedDomainEvent(string userId,
    string serverUrl) : IDomainEvent
{
}