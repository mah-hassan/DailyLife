using DailyLife.Domain.Entities;
using DailyLife.Domain.Primitives;
using DailyLife.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DailyLife.Infrastructure.Data.Business.Configuration;

internal sealed class ReviewConfiguration
    : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.Property(r => r.Id)
            .HasConversion(id => id.Value,
            v => new Id(v))
            .IsRequired();
        
        builder.Property(r => r.BusinessId)
            .HasConversion(bId => bId.Value,
            v => new Id(v))
            .IsRequired();

        builder.Property(r => r.Comment)
            .HasConversion(comment => comment.Value,
            v => Comment.Create(v))
            .IsRequired(false);

    }
}
