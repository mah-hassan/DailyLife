using DailyLife.Domain.Entities;
using DailyLife.Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DailyLife.Infrastructure.Data.Business.Configuration
{
    internal sealed class FavoriteBusinessesConfiguration
        : IEntityTypeConfiguration<FavoriteBusiness>, IBusinessConfiguration
    {
        public void Configure(EntityTypeBuilder<FavoriteBusiness> builder)
        {
            builder.HasKey(fb => new { fb.BusinessId, fb.UserId });

            builder.Property(fb => fb.BusinessId)
                .HasConversion(bId => bId.Value,
                v => new Id(v));

        }
    }
}
