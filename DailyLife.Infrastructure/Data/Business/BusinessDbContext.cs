using DailyLife.Application.Abstractions;
using DailyLife.Domain.Entities;
using DailyLife.Infrastructure.Data.Business.Configuration;
using Microsoft.EntityFrameworkCore;

namespace DailyLife.Infrastructure.Data.Business;

public sealed class BusinessDbContext : DbContext , IApplicationDbContext
{
    public BusinessDbContext(DbContextOptions<BusinessDbContext> options)
        : base(options)
    {
            
    }
    public DbSet<FavoriteBusiness> FavoriteBusinesses { get; set; }
    public DbSet<BusinessAggregate> Businesses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Review> Reviews { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Business");
        //modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly,
        //    config => config.IsAssignableTo<IBusinessConfiguration>() );
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly,
            config => typeof(IBusinessConfiguration).IsAssignableFrom(config));

        modelBuilder.ApplyConfiguration(new BusinessConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
