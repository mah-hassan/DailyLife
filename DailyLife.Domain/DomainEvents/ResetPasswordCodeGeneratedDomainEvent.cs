using DailyLife.Domain.Primitives;
using DailyLife.Domain.ValueObjects;

namespace DailyLife.Domain.DomainEvents;

public record ResetPasswordCodeGeneratedDomainEvent(string code, string email, FullName fullName)
    : IDomainEvent;
  