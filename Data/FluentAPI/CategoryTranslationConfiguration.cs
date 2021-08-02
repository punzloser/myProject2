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
    public class CategoryTranslationConfiguration : IEntityTypeConfiguration<CategoryTranslation>
    {
        public void Configure(EntityTypeBuilder<CategoryTranslation> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();

            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.SeoDescription)
                .HasMaxLength(300);

            builder.Property(p => p.SeoTitle)
                .HasMaxLength(100);

            builder.Property(p => p.SeoAlias)
                .HasMaxLength(200);

            builder.Property(p => p.LanguageId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsRequired();

            builder
                .HasOne(p => p.Category)
                .WithMany(p => p.CategoryTranslations)
                .HasForeignKey(p => p.CategoryId);

            builder
                .HasOne(p => p.Language)
                .WithMany(p => p.CategoryTranslations)
                .HasForeignKey(p => p.LanguageId);
        }
    }
}
