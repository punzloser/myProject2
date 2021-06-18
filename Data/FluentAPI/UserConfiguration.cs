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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.FirstName).HasMaxLength(100).IsRequired();

            builder.Property(p => p.LastName).HasMaxLength(100).IsRequired();

            builder.Property(p => p.Dob).IsRequired();
        }
    }
}
