using DailyLife.Application.Abstractions;
using DailyLife.Application.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DailyLife.Application.Review.Queries.Pagenate;

internal sealed class GetBusinessReviewsQueryHandiler
    : IQueryHandiler<GetBusinessReviewsQuery, PagenationResponse<ReviewResponse>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IBusinessRepository _businessRepository;
    public GetBusinessReviewsQueryHandiler(IApplicationDbContext dbContext, IBusinessRepository businessRepository)
    {
        _dbContext = dbContext;
        _businessRepository = businessRepository;
    }

    public async Task<Result<PagenationResponse<ReviewResponse>>> Handle(GetBusinessReviewsQuery request, CancellationToken cancellationToken)
    {
        var business = await _businessRepository.GetById(new Id(request.businessId));
        if (business is null)
            return Result.Failure<PagenationResponse<ReviewResponse>>(StatusCodes.Status404NotFound,
            new Error(nameof(request.businessId),
             $"business with Id: {request.businessId} was not found"));
        
        var totalCount = await _dbContext.Reviews
                            .CountAsync(r => r.BusinessId == business.Id);
        if (totalCount == 0)
            return Result.Failure<PagenationResponse<ReviewResponse>>(StatusCodes.Status404NotFound,
               new Error("Result", $"No Reviews Was Found for {business.Name}"));

        var result = await _dbContext.Reviews
                    .Where(r => r.BusinessId == business.Id)
                    .Skip((request.pageNumber - 1) * request.size)
                    .Take(request.size)
                    .Select(r => new ReviewResponse(r.Id.Value, r.Rate, r.Comment.Value))
                    .ToListAsync();
        var response = new PagenationResponse<ReviewResponse>
        {
            CurrentPage = request.pageNumber,
            TotalCount = totalCount,
            Result = result,
            HasNextPage = totalCount > (request.pageNumber * request.size)
        };
        return response;
    }
}
