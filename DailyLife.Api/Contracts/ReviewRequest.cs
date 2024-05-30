using System.ComponentModel.DataAnnotations;

namespace DailyLife.Api.Contracts;

public sealed record ReviewRequest(
    [Range(1,5,ErrorMessage ="Rate must be in range 1 to 5")]
    int rate,
    string? comment,
    Guid businessId);
