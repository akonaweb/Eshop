using Eshop.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Eshop.Persistence
{
    [ExcludeFromCodeCoverage]
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey("CategoryId")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);
        }
    }
}