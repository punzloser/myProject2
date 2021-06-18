using Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.FluentAPI
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).UseIdentityColumn();

            builder.Property(p => p.Price).IsRequired();

            builder.Property(p => p.OriginalPrice).IsRequired();

            builder.Property(p => p.Stock)
                .IsRequired()
                .HasDefaultValue<int>(0);

            builder.Property(p => p.ViewCount)
                .IsRequired()
                .HasDefaultValue<int>(0);
        }
    }
}
