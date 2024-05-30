using DailyLife.Domain.Entities;
using DailyLife.Domain.Primitives;
using DailyLife.Domain.Repositories;
using DailyLife.Infrastructure.Data.Business;

namespace DailyLife.Infrastructure.Repositories;

internal sealed class ReviewRepository : IReviewRepository
{
    private readonly BusinessDbContext _dbContext;

    public ReviewRepository(BusinessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Review review) 
        => _dbContext.Add(review);

    public async Task<Review?> GetById(Id id) 
        => await _dbContext.Reviews.FindAsync(id);

    public void Remove(Review review) 
        => _dbContext.Remove(review);
}
