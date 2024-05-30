using DailyLife.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DailyLife.Application.Review.Queries.GetBusinessReviewsSummary;

internal sealed class GetBusinessReviewsSummaryQueryHandiler
   : IQueryHandiler<GetBusinessReviewsSummaryQuery, ReviewSummary>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IBusinessRepository _businessRepository;
    public GetBusinessReviewsSummaryQueryHandiler(IApplicationDbContext dbContext, IBusinessRepository businessRepository)
    {
        _dbContext = dbContext;
        _businessRepository = businessRepository;
    }

    public async Task<Result<ReviewSummary>> Handle(GetBusinessReviewsSummaryQuery request, CancellationToken cancellationToken)
    {
        var business = await _businessRepository.GetById(new Id(request.businessId));
        if (business is null)
            return Result.Failure<ReviewSummary>(StatusCodes.Status404NotFound,
            new Error(nameof(request.businessId),
             $"business with Id: {request.businessId} was not found"));

        var totalCount = await _dbContext.Reviews
                            .CountAsync(r => r.BusinessId == business.Id);
        if (totalCount == 0)
            return new ReviewSummary(0, 0f);
            
        var averageRate = (float)Math.Round(await  _dbContext.Reviews
                            .AverageAsync(r => r.Rate), 2);

        return new ReviewSummary(totalCount, averageRate);
    }
}
