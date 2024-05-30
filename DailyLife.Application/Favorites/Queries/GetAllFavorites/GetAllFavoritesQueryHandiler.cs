using DailyLife.Application.Helpers;
using DailyLife.Application.Shared.Dtos;
using Microsoft.AspNetCore.Http;

namespace DailyLife.Application.Favorites.Queries.GetAllFavorites;

internal sealed class GetAllFavoritesQueryHandiler(
    IBusinessRepository businessRepository,
    Mapper mapper,
    IFavoritesRepository favoritesRepository,
    IHttpContextAccessor contextAccessor)
    : IQueryHandiler<GetAllFavoritesQuery, List<BusinessDetails>>
{
    public async Task<Result<List<BusinessDetails>>> Handle(GetAllFavoritesQuery request, CancellationToken cancellationToken)
    {
        var userId = contextAccessor.GetUserId();
        var favorites = await favoritesRepository.GetAllFavorites(userId);
        if(!favorites.Any())
        {
            return Result.Failure<List<BusinessDetails>>(StatusCodes.Status204NoContent,
            new Error("favorites", "favorites list for this user is empty"));
        }
        var result = new List<BusinessDetails>();
        foreach (var favorite in favorites)
        {
            var businss = await businessRepository
                    .GetById(favorite.BusinessId);
            if (businss is null)
            {
                continue;
            }
            result.Add(mapper.ToBusinessDetails(businss));
        }
        return result;
    }
}
