using Eshop.Domain;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Persistence
{
    internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItem");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Price);
            builder.Property(x => x.ProductId);
            builder.Property(x => x.Quantity);
            builder.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId);
        }
    }
}
