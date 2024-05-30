using DailyLife.Domain.DomainEvents;
using DailyLife.Domain.Errors;
using DailyLife.Domain.Primitives;
using DailyLife.Domain.Shared;
using DailyLife.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace DailyLife.Domain.Entities;

public class AppUser : IdentityUser
{
    public FullName FullName { get; private set; }
    public DateOfBirth DateOfBirth { get; private set; }
    public string? ProfilePicture { get; set; }

    private List<IDomainEvent> _events = new();
    public IReadOnlyCollection<IDomainEvent> GetEvents() => _events.ToList();
    public void ClearDomainEvents() => _events.Clear();
    public void RaiseDomainEvent(IDomainEvent domainEvent)
        => _events.Add(domainEvent);

    public static Result<AppUser> Create(string email, string fullName, DateOnly dateOfBirth)
    {
        var fullNameResult = FullName.Create(fullName);
        var dateOfBirthResult = DateOfBirth.Create(dateOfBirth);

        if (fullNameResult.IsFailure || dateOfBirthResult.IsFailure)
        {
            var errors = new List<Error>();
            if (fullNameResult.IsFailure)
            {
                errors.AddRange(fullNameResult.Errors);
            }
            if (dateOfBirthResult.IsFailure)
            {
                errors.AddRange(dateOfBirthResult.Errors);
            }

            return Result.Failure<AppUser>(400, errors.ToArray());
        }

        var user = new AppUser()
        {
            FullName = fullNameResult.Value,
            DateOfBirth = dateOfBirthResult.Value,
            Email = email,
            Id = Guid.NewGuid().ToString(),
            UserName = fullName,
        };

        return user;
    }
    public void ForgetPassword(string code)
    {
        RaiseDomainEvent(new ResetPasswordCodeGeneratedDomainEvent(code,
            Email!, FullName));  
    }
}
