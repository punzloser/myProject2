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
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();
            builder
                .HasOne(p => p.Product)
                .WithMany(p => p.Carts)
                .HasForeignKey(p => p.ProductId);
            builder
                .HasOne(p => p.User)
                .WithMany(p => p.Carts)
                .HasForeignKey(p => p.UserId);
        }
    }
}
