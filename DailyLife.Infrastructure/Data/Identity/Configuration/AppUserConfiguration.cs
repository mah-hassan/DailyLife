using DailyLife.Domain.Entities;
using DailyLife.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DailyLife.Infrastructure.Data.Identity.Configuration;

internal class AppUserConfiguration 

    : IIdentityConfiguration ,
    IEntityTypeConfiguration<AppUser>
   
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(u => u.FullName)
            .HasConversion(n => n.Value ,
                value => FullName.Create(value).Value)
            .IsRequired();

        builder.Property(u => u.DateOfBirth)
            .HasConversion(date => date.Value,
                value => DateOfBirth.Create(value).Value)   
            .IsRequired();

        builder.Property(u => u.UserName)
            .IsRequired(false);
    }
}