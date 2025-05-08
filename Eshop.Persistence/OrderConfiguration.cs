using Eshop.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Eshop.Persistence
{
    [ExcludeFromCodeCoverage]
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");
            builder.HasKey(x => x.Id);

            builder.Property(o => o.Customer).HasMaxLength(50);
            builder.Property(o => o.Address).HasMaxLength(500);
            builder.Property(o => o.CreatedAt);

            builder.OwnsMany(o => o.Items, i =>
            {
                i.ToTable("OrderItem");
                i.HasKey("OrderId", "ProductId");

                i.Property(oi => oi.Quantity);
                i.Property(oi => oi.Price).HasColumnType("decimal(18,2)");

                i.HasOne(oi => oi.Product)
                    .WithMany()
                    .HasForeignKey("ProductId")
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
