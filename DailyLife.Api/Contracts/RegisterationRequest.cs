using DailyLife.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace DailyLife.Api.Contracts;

public sealed record RegistrationRequest(
    [EmailAddress]string email,
    string password,
    DateOnly dateOfBirth,
    string fullname,
    [EnumDataType(typeof(AppRoles),
    ErrorMessage = "Only 1, 2, 3 Are Allawed")]
    AppRoles role);
