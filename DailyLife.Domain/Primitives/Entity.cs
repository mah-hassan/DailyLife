using DailyLife.Domain.Errors;
using DailyLife.Domain.Shared;

namespace DailyLife.Domain.Primitives;

public abstract class Entity : IEquatable<Entity>
{
    public Id Id { get; init; }

    protected Entity(Id id)
    {
        if (string.IsNullOrEmpty(id.Value.ToString()))
        {
            throw new ArgumentNullException(DomainErrors.Id.NotEmptyOrNull.ToString(), nameof(id));
        }
        Id = id;
    }

    public bool Equals(Entity? other)
    {
        return other is { } ? Id == other.Id : false;
    }
    public override bool Equals(object? obj)
    {
        if (!(obj is Entity entity)) return false;
        return Equals(entity);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Id);
    }
}
