using DailyLife.Application.Abstractions.Messaging;

namespace DailyLife.Application.Review.Queries.GetBusinessReviewsSummary;

public sealed record GetBusinessReviewsSummaryQuery(Guid businessId)
    : IQuery<ReviewSummary>;
