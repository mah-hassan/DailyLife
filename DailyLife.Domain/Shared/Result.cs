using DailyLife.Domain.Errors;

namespace DailyLife.Domain.Shared;
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public int StatusCode { get; }
    public IEnumerable<Error> Errors { get; }

    protected internal Result(bool isSuccess, int statusCode, params Error[] errors)
    {
        if (isSuccess && errors.Length > 0)
        {
            throw new InvalidOperationException("Success results cannot have errors");
        }
        if (!isSuccess && errors.Length == 0)
        {
            throw new InvalidOperationException("Failure results must have errors");
        }
        if (isSuccess && !(statusCode >= 200 && statusCode < 300))
        {
            throw new ArgumentException($"{nameof(statusCode)} of a success result must be in range 200 to 300 ");
        }
        IsSuccess = isSuccess;
        StatusCode = statusCode;
        Errors = errors;
    }

    public static Result Success() => new(true, 200);
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, 200);

    public static Result Failure(int statusCode, params Error[] errors) => new(false, statusCode, errors);

    public static Result<TValue> Failure<TValue>(int statusCode, params Error[] errors) => new(default, false, statusCode, errors);

    public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null
            ? Success(value)
            : Failure<TValue>(400,Error.NullValue);
    public static Result<TValue> Create<TValue>(TValue? value, int statusCode, params Error[] errors) =>
        value is not null
            ? Success(value)
            : Failure<TValue>(statusCode, errors);

    //public static Result Create(bool isSuccess, Error error, int statusCode) =>
    //    new(isSuccess, statusCode, error);

    public static Result Create(bool isSuccess, int statusCode, params Error[] errors) =>
        new(isSuccess, statusCode, errors);
}
