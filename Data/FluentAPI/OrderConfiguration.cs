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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();

            builder.Property(p => p.OrderDate).HasDefaultValue(DateTime.Now);

            builder.Property(p => p.ShipName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.ShipAddress)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.ShipEmail)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(p => p.ShipPhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .HasOne(p => p.User)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.UserId);
        }
    }
}
