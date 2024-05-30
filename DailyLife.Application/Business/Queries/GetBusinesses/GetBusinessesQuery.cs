using DailyLife.Application.Shared.Dtos;

namespace DailyLife.Application.Business.Queries.GetBusinesses;

public sealed record GetBusinessesQuery(int page, int size, string? fillter,
    decimal? latitude, decimal? longitude)
    : IQuery<PagenationResponse<BusinessDetails>>;