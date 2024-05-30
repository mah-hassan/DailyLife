using DailyLife.Domain.Entities;
using DailyLife.Domain.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Threading;

namespace DailyLife.Infrastructure.Repositories.Caching;

internal class CachedCategoryRepository(
    IMemoryCache memoryCache,
    CategoryRepository decorated)
    : ICategoryRepository
{
    public void Add(Category entity)
        => decorated.Add(entity);
    public void Delete(Category entity)
        => decorated.Delete(entity);

    public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await memoryCache.GetOrCreateAsync<List<Category>>(
            "categories",  x =>
            {
                x.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return decorated.GetAllAsync(cancellationToken);
            });
        return result!;
    }

    public async Task<Category?> GetById(Id id)
    {
        var result = await memoryCache.GetOrCreateAsync<Category?>(
            $"category-{id}", x =>
            {
                x.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return decorated.GetById(id);
            });
        return result;
    }

    public async Task<bool> IsNameExsists(string name)
        => await decorated.IsNameExsists(name);

    public void Update(Category entity)
        => decorated.Update(entity);
}
