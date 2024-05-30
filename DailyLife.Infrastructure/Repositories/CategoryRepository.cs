using DailyLife.Domain.Entities;

using DailyLife.Domain.Repositories;
using DailyLife.Infrastructure.Data.Business;
using Microsoft.EntityFrameworkCore;

namespace DailyLife.Infrastructure.Repositories;

internal sealed class CategoryRepository :
  BaseRepository<Category>, ICategoryRepository 
{
    private readonly BusinessDbContext _dbContext;

    public CategoryRepository(BusinessDbContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsNameExsists(string name)
        => await _dbContext.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower());   
}
