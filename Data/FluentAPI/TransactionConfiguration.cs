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
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .UseIdentityColumn();

            builder
                .HasOne(p => p.User)
                .WithMany(p => p.Transactions)
                .HasForeignKey(p => p.UserId);
        }
    }
}
