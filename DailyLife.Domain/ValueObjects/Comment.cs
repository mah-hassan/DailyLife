using DailyLife.Domain.Errors;
using DailyLife.Domain.Shared;

namespace DailyLife.Domain.ValueObjects;
public sealed record Comment
{
    private const int _maxLength = 750;
    public string? Value { get; init; }

    private Comment(string? value)
    {
        if (value?.Length is > 750) 
            throw new InvalidOperationException($"comment max length is {_maxLength}");
        Value = value;
    }
    public static Result<Comment> Create(string? value)
    {
        if (value?.Length is > 750)
            return Result.Failure<Comment>(400,
                new Error("Comment",
                $"comment max length is {_maxLength}"));
        return new Comment(value);
    }
}
