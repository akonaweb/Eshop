using Eshop.Domain;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Persistence
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.OrderDate);
            builder.Property(x => x.OrderItems);
            builder.Property(x => x.Status);
            builder.Property(x => x.TotalPrice);
        }
    }
}
