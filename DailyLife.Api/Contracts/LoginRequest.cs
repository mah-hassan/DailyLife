using System.ComponentModel.DataAnnotations;

namespace DailyLife.Api.Contracts;

public sealed record LoginRequest(
    [EmailAddress]string email,
    string password);
