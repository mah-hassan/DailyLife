using DailyLife.Domain.Errors;
using DailyLife.Domain.Shared;

namespace DailyLife.Domain.ValueObjects;

public sealed record DateOfBirth
{
    private static DateTime _utcNow = DateTime.UtcNow;
    private const int _minmumAge = 18;
    private DateOfBirth(DateOnly value) => Value = value;
 
    public DateOnly Value { get; }
    public static Result<DateOfBirth> Create(DateOnly value)
    {
        if (value.Year > _utcNow.Year &&
            (_utcNow.Year - value.Year) >= _minmumAge )
        {
            return Result.Failure<DateOfBirth>(400, DomainErrors.DateOfBirth.InvalidDate);
        }
        return new DateOfBirth(value);
    }
}
