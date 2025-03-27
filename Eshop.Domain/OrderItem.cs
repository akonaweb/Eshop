namespace Eshop.Domain
{
    public class OrderItem
    {
        public int Id { get; }
        public Product Product { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        public int OrderId { get; private set; }
        public int ProductId { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private OrderItem() { } // EF Core requires this
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public OrderItem(int id, Product product, int quantity, decimal price, int orderId, int productId)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be a non-negative value.");

            ValidateParameters(product, quantity, price);
            Id = id;
            Product = product ?? throw new ArgumentNullException(nameof(product));
            Quantity = quantity;
            Price = price;
            OrderId = orderId;
            ProductId = productId;
        }

        public void Update(Product product, int quantity, decimal price)
        {
            ValidateParameters(product, quantity, price);
            Product = product ?? throw new ArgumentNullException(nameof(product));
            Quantity = quantity;
            Price = price;
            ProductId = product.Id;
        }

        private static void ValidateParameters(Product product, int quantity, decimal price)
        {
            if (product == null) throw new ArgumentNullException(nameof(product), "Product cannot be null");
            if (quantity <= 0) throw new ArgumentException("Invalid quantity", nameof(quantity));
            if (price <= 0) throw new ArgumentException("Invalid price", nameof(price));
        }
    }
}
