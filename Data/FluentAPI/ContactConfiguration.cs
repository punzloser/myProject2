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
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();
            builder.Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(p => p.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(p => p.Message)
                .IsRequired();
        }
    }
}
