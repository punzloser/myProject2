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
    public class ProductTranslationConfiguration : IEntityTypeConfiguration<ProductTranslation>
    {
        public void Configure(EntityTypeBuilder<ProductTranslation> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();

            builder
                .HasOne(p => p.Product)
                .WithMany(p => p.ProductTranslations)
                .HasForeignKey(p => p.ProductId);

            builder
                .HasOne(p => p.Language)
                .WithMany(p => p.ProductTranslations)
                .HasForeignKey(p => p.LanguageId);

            builder.Property(p => p.LanguageId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.SeoAlias).IsRequired(false)
                .HasMaxLength(200);

            builder.Property(p => p.SeoDescription).IsRequired(false)
                .HasMaxLength(300);

            builder.Property(p => p.SeoTitle).IsRequired(false)
                .HasMaxLength(100);

            builder.Property(p => p.Description).IsRequired(false)
                .HasMaxLength(300);

            builder.Property(p => p.Details).IsRequired(false)
                .HasMaxLength(500);
        }
    }
}
