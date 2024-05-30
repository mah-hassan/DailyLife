using DailyLife.Domain.Entities;
using DailyLife.Domain.Primitives;

namespace DailyLife.Domain.Repositories;

public interface IFavoritesRepository
{
    Task<List<FavoriteBusiness>> GetAllFavorites(Guid userId);
    void AddFavoriteBusiness(FavoriteBusiness favorite);
    void RemoveFromFavorites(FavoriteBusiness favorite);
    Task<FavoriteBusiness?> GetFavorite(Guid userId, Id businessId);
    Task<IReadOnlyList<FavoriteBusiness>> GetAllFavoritesForAspecificBusiness(Id businessId, CancellationToken cancellationToken);
}
