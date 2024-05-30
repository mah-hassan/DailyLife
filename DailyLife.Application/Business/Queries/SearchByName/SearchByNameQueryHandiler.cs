using DailyLife.Application.Helpers;
using DailyLife.Application.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DailyLife.Application.Business.Queries.SearchByName;

internal sealed class SearchByNameQueryHandiler(IBusinessRepository businessRepository, Mapper mapper)
        : IQueryHandiler<SearchByNameQuery, List<BusinessDetails>>
{
    public async Task<Result<List<BusinessDetails>>> Handle(SearchByNameQuery request, CancellationToken cancellationToken)
    {

        if (string.IsNullOrWhiteSpace(request.searchTerm))
        {
            return Result
                .Failure<List<BusinessDetails>>(StatusCodes.Status400BadRequest,
                new Error(nameof(request.searchTerm), $"{nameof(request.searchTerm)} can not be null or white spaces"));
        }

        var result = await businessRepository
           .SearchByName(request.searchTerm.Trim()).ToListAsync();
        return result.Any() ? result.Select(b => mapper.ToBusinessDetails(b)).ToList() :
            Result.Failure<List<BusinessDetails>>(StatusCodes.Status404NotFound,
                new Error("Result", $"{request.searchTerm}..No businesses was found"));

    }
}
