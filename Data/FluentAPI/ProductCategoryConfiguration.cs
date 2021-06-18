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
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasKey(p => new { p.ProductID, p.CategoryID });
            builder
                .HasOne(p => p.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(p => p.ProductID);
            builder
                .HasOne(p => p.Category)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(p => p.CategoryID);
        }
    }
}
