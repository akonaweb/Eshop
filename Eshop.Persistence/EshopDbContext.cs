using Eshop.Domain;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Persistence
{
    public class EshopDbContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public EshopDbContext(DbContextOptions<EshopDbContext> options) : base(options) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public DbSet<Category> Categories { get; private set; }
        public IQueryable<Category> CategoriesViews => Categories.AsNoTracking();
        public DbSet<Product> Products { get; private set; }
        public IQueryable<Product> ProductsViews => Products.AsNoTracking();
        public DbSet<Order> Orders { get; private set; }
        public IQueryable<Order> OrdersViews => Orders.AsNoTracking();
        public DbSet<OrderItem> OrderItems { get; private set; }
        public IQueryable<OrderItem> OrderItemsViews => OrderItems.AsNoTracking();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new OrderConfiguration());
            builder.ApplyConfiguration(new OrderItemConfiguration());
        }
    }
}

