namespace DailyLife.Application.Review.Commands.Update;

public sealed record UpdateReviewCommand(Guid id, int rate, string? comment)
    : ICommand;
