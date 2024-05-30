using System.ComponentModel.DataAnnotations;

namespace DailyLife.Api.Contracts;

public sealed record ResetPasswordRequest(
    [EmailAddress]string email,
    string newPassword,
    [Length(6,6)]string code);

