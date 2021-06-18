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
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .IsUnicode(false)
                .HasMaxLength(10);

            builder.Property(p => p.Name)
                .HasMaxLength(20)
                .IsRequired();

        }
    }
}
