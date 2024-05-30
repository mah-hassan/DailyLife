
using DailyLife.Domain.Errors;
namespace DailyLife.Domain.Shared;

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    internal Result(TValue? value, bool isSuccess, int statusCode, params Error[] errors)
        : base(isSuccess, statusCode, errors)
    {
        _value = value;
    }

    public TValue Value =>
        IsSuccess
            ? _value ?? throw new InvalidOperationException("Success result does not contain a value")
            : throw new InvalidOperationException("Failure result does not contain a value");

    public static implicit operator Result<TValue>(TValue? value) => Create(value);

    public static implicit operator TValue(Result<TValue> result) => result.Value;
}