using DailyLife.Domain.Entities;
using DailyLife.Infrastructure.Data.Identity.Configuration;
using DailyLife.Infrastructure.Outbox;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DailyLife.Infrastructure.Data.Identity;

public class AppIdentityDbContext : IdentityDbContext<AppUser>
{
    internal DbSet<OutboxMessage> OutboxMessages { get; set; }
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
        : base(options)
    {
            
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly,
            type => typeof(IIdentityConfiguration).IsAssignableFrom(type));
        base.OnModelCreating(builder);
    }
}