using Microsoft.AspNetCore.Identity;

namespace DailyLife.Application.Identity.Commands.ResetPassword;

public sealed record ResetPasswordCommand(string email, string newPassword, string code)
    : ICommand<IdentityResult>;
