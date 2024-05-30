using DailyLife.Domain.Entities;
using DailyLife.Domain.Primitives;
using DailyLife.Domain.Repositories;
using DailyLife.Infrastructure.Data.Business;
using Microsoft.EntityFrameworkCore;

namespace DailyLife.Infrastructure.Repositories;

internal sealed class FavoritesRepository(BusinessDbContext context) : IFavoritesRepository
{
    public void AddFavoriteBusiness(FavoriteBusiness favorite)
        => context.FavoriteBusinesses.Add(favorite);

    public async Task<List<FavoriteBusiness>> GetAllFavorites(Guid userId) 
        => await context.FavoriteBusinesses.Where(f => f.UserId == userId).ToListAsync();

    public async Task<IReadOnlyList<FavoriteBusiness>> GetAllFavoritesForAspecificBusiness(Id businessId, CancellationToken cancellationToken)
        => await context.FavoriteBusinesses.Where(f => f.BusinessId == businessId).ToListAsync(cancellationToken);

    public async Task<FavoriteBusiness?> GetFavorite(Guid userId, Id businessId) 
        => await context.FavoriteBusinesses.FirstOrDefaultAsync(f => f.BusinessId == businessId && f.UserId == userId);

    public void RemoveFromFavorites(FavoriteBusiness favorite) 
        => context.FavoriteBusinesses.Remove(favorite);
}
