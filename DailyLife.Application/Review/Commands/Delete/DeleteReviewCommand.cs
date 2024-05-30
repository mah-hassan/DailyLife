namespace DailyLife.Application.Review.Commands.Delete;

public sealed record DeleteReviewCommand(Guid reviewId)
    : ICommand;
