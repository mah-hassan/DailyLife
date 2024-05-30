namespace DailyLife.Application.Review.Commands.Create;

public sealed record CreateReviewCommand(int rate, string? comment, Guid businessId)
    : ICommand<Guid>;
