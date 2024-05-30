namespace DailyLife.Application.Identity.Commands.Login;

public sealed record LoginCommand(string email, string password)
    : ICommand<LoginResponse>;
