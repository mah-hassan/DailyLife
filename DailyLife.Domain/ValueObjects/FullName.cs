using DailyLife.Domain.Errors;
using DailyLife.Domain.Shared;
using System.Text.RegularExpressions;

namespace DailyLife.Domain.ValueObjects;

public sealed record FullName
{
    private const int _maxLength = 100; 
    public string Value { get; } = string.Empty;
    private FullName(string value) => Value = value;

    public static Result<FullName> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Result.Failure<FullName>(400,Error.NullValue);

        if (value.Length > _maxLength)
            return Result.Failure<FullName>(400, DomainErrors.FullName.InvalidLength);

        if (!Regex.IsMatch(value, @"^[A-Za-z\u0600-\u06FF]+(?: [A-Za-z\u0600-\u06FF]+)*$"))
            return Result.Failure<FullName>(400, DomainErrors.FullName.InvalidValue);


        return new FullName(value);
    }
}
