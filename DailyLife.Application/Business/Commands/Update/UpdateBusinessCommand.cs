using DailyLife.Domain;

namespace DailyLife.Application.Business.Commands.Update;

public sealed record UpdateBusinessCommand(
    Guid id,
    string name,
    string? description,
    decimal latituade,
    decimal longituade,
    string city,
    string state,
    string street,
    string? addressDescription,
    List<WorkTime> workTimes) : ICommand;
