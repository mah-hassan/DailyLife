namespace DailyLife.Application.Category.Commands.Delete;
public sealed record DeleteCategoryCommand(Guid id)
    : ICommand;
