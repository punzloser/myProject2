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
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).UseIdentityColumn();
            builder.Property(a => a.ImagePath).HasMaxLength(200);
            builder.Property(a => a.Caption).HasMaxLength(100).IsRequired(false);
            builder
               .HasOne(a => a.Product)
               .WithMany(a => a.ProductImages)
               .HasForeignKey(a => a.ProductId);

        }
    }
}
