using DailyLife.Domain;

namespace DailyLife.Api.Contracts;


public sealed record UpdateBusinessRequest(
    string name,
    string? description,
    decimal latituade,
    decimal longituade,
    string city,
    string state,
    string street,
    string? addressDescription,
    List<WorkTime> workTimes);

