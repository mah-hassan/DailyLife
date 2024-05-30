using DailyLife.Application.Abstractions.Messaging;
using DailyLife.Application.Shared.Dtos;

namespace DailyLife.Application.Review.Queries.Pagenate;

public sealed record GetBusinessReviewsQuery(Guid businessId,
         int pageNumber = 0, int size = 10)
         : IQuery<PagenationResponse<ReviewResponse>>;
