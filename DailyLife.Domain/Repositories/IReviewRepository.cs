using DailyLife.Domain.Entities;
using DailyLife.Domain.Primitives;

namespace DailyLife.Domain.Repositories;

public interface IReviewRepository
{
    void Add(Review review);
    Task<Review?> GetById(Id id);
    void Remove(Review review);
}
