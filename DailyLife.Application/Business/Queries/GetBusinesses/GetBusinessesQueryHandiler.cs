using DailyLife.Application.Abstractions;
using DailyLife.Application.Helpers;
using DailyLife.Application.Helpers.Constants;
using DailyLife.Application.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DailyLife.Application.Business.Queries.GetBusinesses;

internal sealed class GetBusinessesQueryHandiler(IBusinessRepository businessRepository,
        IApplicationDbContext context,
        Mapper mapper)
        : IQueryHandiler<GetBusinessesQuery, PagenationResponse<BusinessDetails>>
{
    public async Task<Result<PagenationResponse<BusinessDetails>>> Handle(GetBusinessesQuery request, CancellationToken cancellationToken)
    {

        int skip = (request.page - 1) * request.size;

        switch (request.fillter?.ToLower())
        {
            case Fillters.Distance:
                return await GetNearest(request);
            case Fillters.Rate:
                return await PrepareResult(await businessRepository
                    .GetTopRated(skip,
                    request.size),
                    request.page, request.size);
            case Fillters.Active:
                return await GetActive(request);
            default:
                break;
        }

        IQueryable<BusinessAggregate> query =
             context.Businesses.AsQueryable();
        

        var result = await query
            .Skip(skip)
            .Take(request.size)
            .ToListAsync();
       
        return await PrepareResult(result, request.page, request.size);

    }

    private async Task<Result<PagenationResponse<BusinessDetails>>> GetActive(GetBusinessesQuery request)
    {
        var result =  await businessRepository.GetActive()
            .Skip((request.page - 1) * request.size)
            .Take(request.size)
            .ToListAsync();
        return await PrepareResult(result, request.page, request.size);

    }

    private async Task<Result<PagenationResponse<BusinessDetails>>> GetNearest(GetBusinessesQuery request)
    {
        if (!request.latitude.HasValue || !request.longitude.HasValue)
        {
            return Result.Failure<PagenationResponse<BusinessDetails>>(
            StatusCodes.Status400BadRequest,
            new Error("Coordinates",
            "for distance fillter latitude and longitude both are required"));

        }
        var disFillterdResult = await businessRepository.GetNearest((decimal)request.latitude,
            (decimal)request.longitude,
            ((request.page - 1) * request.size), request.size).ToListAsync();
        return await PrepareResult(disFillterdResult, request.page, request.size);
    }

    private async Task<Result<PagenationResponse<BusinessDetails>>> PrepareResult(List<BusinessAggregate> businesses,
        int page, int size)
    {
        if (!businesses.Any())
        {
            return Result.Failure<PagenationResponse<BusinessDetails>>(StatusCodes.Status404NotFound,
                new Error("Result", "No businesses was found"));
        }
        int count = await context.Businesses.CountAsync();
        return new PagenationResponse<BusinessDetails>
        {
            TotalCount = count,
            CurrentPage = page,
            HasNextPage = count > (page * size),
            Result = businesses.Select(b => mapper.ToBusinessDetails(b)).ToList()
        };
    }
}
