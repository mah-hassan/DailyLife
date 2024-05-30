namespace DailyLife.Application.Business.Commands.Delete;

public sealed record DeleteBusinessCommand(Guid id)
    : ICommand;
