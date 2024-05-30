using DailyLife.Domain.Primitives;

namespace DailyLife.Domain.Repositories;

public interface IBaseRepository<TEntity>
{
    Task<TEntity?> GetById(Id id);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);
}
