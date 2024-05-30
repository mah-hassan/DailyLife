namespace DailyLife.Application.Identity.Commands.ForgetPassword;

public sealed record ForgetPasswordCommand (string email)
    : ICommand;
