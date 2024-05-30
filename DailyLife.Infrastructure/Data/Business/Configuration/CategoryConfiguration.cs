using DailyLife.Domain.Entities;
using DailyLife.Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyLife.Infrastructure.Data.Business.Configuration
{
    internal class CategoryConfiguration
         : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Id)
                .HasConversion(id => id.Value,
                v => new Id(v));
            builder.Property(c => c.Name)
                .HasMaxLength(150);
        }
    }
}
