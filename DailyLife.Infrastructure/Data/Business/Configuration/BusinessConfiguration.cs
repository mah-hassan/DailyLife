using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DailyLife.Infrastructure.Data.Business.Configuration;

internal class BusinessConfiguration
    : IEntityTypeConfiguration<BusinessAggregate>
{
    public void Configure(EntityTypeBuilder<BusinessAggregate> builder)
    {
        builder.Property(b => b.Description)
            .HasMaxLength(500)
            .IsRequired(false);
        builder.Property(b => b.Name)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(b => b.ProfilePicture)
            .HasMaxLength(500)
            .IsRequired(false);
        builder.Property(b => b.Album)
            .HasMaxLength(500)
            .IsRequired(false);
        builder.HasIndex(b => b.Name)
            .IsUnique();
        builder.HasIndex(b => b.OwnerId)
            .IsUnique();
        builder.Property(b => b.Id)
            .HasConversion(id => id.Value,
            v => new Id(v));

        builder.ComplexProperty(b => b.Location, locationBuilder =>
        {
            locationBuilder.Property(l => l.Latituade)
                .HasColumnType("decimal").HasPrecision(12,10);
            locationBuilder.Property(l => l.Longituade)
            .HasColumnType("decimal").HasPrecision(12,10);
        });
        
        builder.HasOne(b => b.Category)
            .WithMany()
            .HasForeignKey(b => b.CategoryId)
            .IsRequired();

        builder.OwnsMany(b => b.WorkTimes);

        builder.ComplexProperty(b => b.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.City)
            .HasMaxLength(100)
            .IsRequired();    
            addressBuilder.Property(a => a.Street)
            .HasMaxLength(100)
            .IsRequired();
            addressBuilder.Property(a => a.Street)
            .HasMaxLength(200)
            .IsRequired();    
            addressBuilder.Property(a => a.Description)
            .HasMaxLength(350)
            .IsRequired(false);
        });

        builder.HasMany(b => b.Reviews)
            .WithOne()
            .HasForeignKey(r => r.BusinessId)
            .IsRequired();
    }
}
