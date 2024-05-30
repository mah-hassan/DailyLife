using DailyLife.Domain.Primitives;
using DailyLife.Infrastructure.Data.Business;
using Microsoft.EntityFrameworkCore;

namespace DailyLife.Infrastructure.Repositories;
internal abstract class BaseRepository<TEntity>
 where TEntity : class
{
    private readonly BusinessDbContext _dbContext;

    protected BaseRepository(BusinessDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public virtual async Task<TEntity?> GetById(Id id)
        => await _dbContext.Set<TEntity>().FindAsync(id);
    public virtual async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        => await _dbContext.Set<TEntity>().ToListAsync(cancellationToken);
    public virtual void Add(TEntity entity)
        =>  _dbContext.Set<TEntity>().Add(entity);
    public virtual void Update(TEntity entity)
        => _dbContext.Set<TEntity>().Update(entity);
    public virtual void Delete(TEntity entity)
        => _dbContext.Set<TEntity>().Remove(entity);
}