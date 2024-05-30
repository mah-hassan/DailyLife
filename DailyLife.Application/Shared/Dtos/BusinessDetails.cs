using DailyLife.Domain;
using DailyLife.Domain.ValueObjects;

namespace DailyLife.Application.Shared.Dtos;

public sealed record BusinessDetails(
    Guid id,
    string name,
    string? description,
    string? profilePicture,
    string?[] album,
    string category,
    Location location,
    Address address,
    IReadOnlySet<WorkTime> WorkTimes);