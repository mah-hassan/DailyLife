using DailyLife.Domain;
using System.ComponentModel.DataAnnotations;

namespace DailyLife.Api.Contracts;
public sealed record BusinessRequest(
    string name,
    string? description,
    [Required]decimal latituade,
    [Required]decimal longituade,
    string city,
    string state,
    string street,
    string? addressDescription,
    [Required]
    Guid categoryId,
    List<WorkTime> workTimes);

