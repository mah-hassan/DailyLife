using DailyLife.Domain.Primitives;

namespace DailyLife.Domain.DomainEvents;

public sealed record BusinessDeletedDomainEvent(Id businessId) : IDomainEvent;
