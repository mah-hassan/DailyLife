using DailyLife.Application.Shared.Dtos;

namespace DailyLife.Application.Favorites.Queries.GetAllFavorites;

public sealed record GetAllFavoritesQuery()
    : IQuery<List<BusinessDetails>>;
