namespace DailyLife.Application.Review.Queries;

public sealed record ReviewResponse(Guid id, int rate, string? comment);
