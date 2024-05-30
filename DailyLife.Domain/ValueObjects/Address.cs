using DailyLife.Domain.Errors;
using DailyLife.Domain.Shared;

namespace DailyLife.Domain.ValueObjects;

public sealed record Address
{
    
    public required string City { get; set; }
    public required string State { get; set; }
    public required string Street { get; set; }
    public string? Description { get; set; }


    public static Result<Address> Create(string city,
        string state,
        string street,
        string? description)
    {
        var validationResult = Validate(city, state, street, description);
        if (validationResult.IsFailure)
        {
            return Result.Failure<Address>(400,
                validationResult.Errors.ToArray());
        }
        return new Address
        {
            City = city,
            State = state,
            Street = street,
            Description = description
        };
    }
    public Result Update(string city,
        string state,
        string street,
        string? description)
    {
        var validationResult = Validate(city, state, street, description);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }
        City = city;
        State = state;
        Street = street;
        Description = description;
        return Result.Success();
    }
    private static Result Validate(string city,
        string state,
        string street,
        string? description)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return Result.Failure<Address>(400,
                new Error(nameof(city),
                "City cannot be empty or whitespace"));
        }
        if (string.IsNullOrWhiteSpace(state))
        {
            return Result.Failure<Address>(400,
                new Error(nameof(state),
                "State cannot be empty or whitespace"));
        }
        if (string.IsNullOrWhiteSpace(street))
        {
            return Result.Failure<Address>(400,
                new Error(nameof(street),
                "Street cannot be empty or whitespace"));
        }
        return Result.Success();
    }
}
