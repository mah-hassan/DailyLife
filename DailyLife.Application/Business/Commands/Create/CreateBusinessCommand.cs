using DailyLife.Domain;



namespace DailyLife.Application.Business.Commands.Create;

public sealed record CreateBusinessCommand(
    string name,
    string? description,
    decimal latituade,
    decimal longituade,
    string city,
    string state,
    string street,
    string? addressDescription,
    Guid categoryId,
    List<WorkTime> workTimes)
    : ICommand;
