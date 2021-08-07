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
    public class ProductSlideConfiguration : IEntityTypeConfiguration<ProductSlide>
    {
        public void Configure(EntityTypeBuilder<ProductSlide> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).UseIdentityColumn();
            builder.Property(a => a.ImageProductSlide).HasMaxLength(200);

            builder
                .HasOne(a => a.Product)
                .WithMany(a => a.ProductSlides)
                .HasForeignKey(a => a.ProductId);
        }
    }
}
