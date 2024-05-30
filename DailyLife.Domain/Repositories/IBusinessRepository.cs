using DailyLife.Domain.Aggregates;
using DailyLife.Domain.Primitives;

namespace DailyLife.Domain.Repositories;

public interface IBusinessRepository
    : IBaseRepository<Business>
{
    Task<bool> OwnerExist(Guid ownerId);
    Task<bool> IsNameUniqe(string name);    
    IQueryable<Business> SearchByName(string searchTerm);
    IQueryable<Business> GetActive();
    Task<List<Business>> GetTopRated(int skip, int take);
    IQueryable<Business> GetNearest(decimal currentLatitude, decimal currentLongitude,
        int skip, int take);
}
