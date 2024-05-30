namespace DailyLife.Application.Category.Commands.Add;

public sealed record AddCategoryCommand(string name)
    : ICommand;
