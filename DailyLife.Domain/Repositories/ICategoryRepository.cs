using DailyLife.Domain.Entities;
using DailyLife.Domain.Primitives;

namespace DailyLife.Domain.Repositories;

public interface ICategoryRepository
  
{
    Task<bool> IsNameExsists(string name);
    Task<Category?> GetById(Id id);
    void Add(Category entity);
    void Update(Category entity);
    void Delete(Category entity);
    Task<List<Category>> GetAllAsync(CancellationToken cancellationToken);
}
