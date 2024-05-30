using DailyLife.Application.Shared.Dtos;

namespace DailyLife.Application.Business.Queries.SearchByName;

public sealed record SearchByNameQuery(
    string? searchTerm) : IQuery<List<BusinessDetails>>;
