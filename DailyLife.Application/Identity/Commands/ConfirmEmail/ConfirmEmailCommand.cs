namespace DailyLife.Application.Identity.Commands.ConfirmEmail;

public record ConfirmEmailCommand(string token, string userId) : ICommand;
