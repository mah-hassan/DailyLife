using Microsoft.EntityFrameworkCore;

namespace DailyLife.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<BusinessAggregate> Businesses { get; }
    DbSet<ReviewEntity> Reviews { get; }
}
