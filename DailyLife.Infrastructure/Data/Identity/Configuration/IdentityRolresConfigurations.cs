using DailyLife.Domain.Entities;
using DailyLife.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DailyLife.Infrastructure.Data.Identity.Configuration
{
    internal class IdentityRolresConfigurations
        : IIdentityConfiguration,
        IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            List<IdentityRole> roles = new()
            {
                new ()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = nameof(AppRoles.Admin),
                    NormalizedName = nameof(AppRoles.Admin).ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new ()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = nameof(AppRoles.BusinessOwner),
                    NormalizedName = nameof(AppRoles.BusinessOwner).ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new ()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = nameof(AppRoles.Default),
                    NormalizedName = nameof(AppRoles.Default).ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            };
            builder.HasData(roles);
        }
    }
}
