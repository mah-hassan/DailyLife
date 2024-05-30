namespace DailyLife.Application.Category.Commands.Update;

public sealed record UpdateCategoryCommand(Guid id, string name)
    : ICommand;
