using DailyLife.Domain.Enums;

namespace DailyLife.Application.Identity.Commands.Register;

public sealed record RegisterUserCommand(
    string email,
    string password,
    DateOnly dateOfBirth,
    string fullname,
    AppRoles role) : ICommand;
