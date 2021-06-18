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
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(p => new { p.OrderId, p.ProductId });
            builder
                .HasOne(p => p.Order)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(p => p.OrderId);
            builder
                .HasOne(p => p.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(p => p.ProductId);
        }
    }
}
