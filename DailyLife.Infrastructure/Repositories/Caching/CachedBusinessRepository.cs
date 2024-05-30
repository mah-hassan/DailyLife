using DailyLife.Domain.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace DailyLife.Infrastructure.Repositories.Caching;

internal sealed class CachedBusinessRepository
    (BusinessRepository decorated,
    IMemoryCache memoryCache)
    : IBusinessRepository
{
    public void Add(BusinessAggregate entity)
        => decorated.Add(entity);
    public void Delete(BusinessAggregate entity)
        => decorated.Delete(entity);
    public IQueryable<BusinessAggregate> GetActive()
        => decorated.GetActive();
    public Task<List<BusinessAggregate>> GetAllAsync(CancellationToken cancellationToken)
        => decorated.GetAllAsync(cancellationToken);
    public async Task<BusinessAggregate?> GetById(Id id)
    {
        return await memoryCache.GetOrCreateAsync($"Business-{id}", x =>
        {
            x.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return decorated.GetById(id);
        });
    }

    public IQueryable<BusinessAggregate> GetNearest(decimal currentLatitude, decimal currentLongitude, int skip, int take)
        => decorated.GetNearest(currentLatitude, currentLongitude, skip, take);

    public async Task<List<BusinessAggregate>> GetTopRated(int skip, int take)
    {
        var result = await memoryCache
            .GetOrCreateAsync("businesses-topRated", async x =>
            {
                x.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);   
                return await decorated.GetTopRated(skip, take);
            });
        return result ?? Enumerable.Empty<BusinessAggregate>().ToList();
    }

    public Task<bool> IsNameUniqe(string name)
        => decorated.IsNameUniqe(name);
    public Task<bool> OwnerExist(Guid ownerId)
        => decorated.OwnerExist(ownerId);   
    public IQueryable<BusinessAggregate> SearchByName(string searchTerm)
        => decorated.SearchByName(searchTerm);
    public void Update(BusinessAggregate entity)
        => decorated.Update(entity);    
}
