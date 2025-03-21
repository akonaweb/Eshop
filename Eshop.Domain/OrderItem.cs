namespace Eshop.Domain
{
    public class OrderItem
    {
        public int Id { get; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        public int OrderId { get; private set; }
        public Order Order { get; private set; } // Navigation property

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private OrderItem() { } // EF Core requires this
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public OrderItem(int id, int productId, int quantity, decimal price, int orderId, Order order)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be a non-negative value.");

            ValidateParameters(productId, quantity, price);
            Id = id;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
            OrderId = orderId;
            Order = order ?? throw new ArgumentNullException(nameof(order));
        }

        public void Update(int productId, int quantity, decimal price)
        {
            ValidateParameters(productId, quantity, price);
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }

        private static void ValidateParameters(int productId, int quantity, decimal price)
        {
            if (productId <= 0) throw new ArgumentException("Invalid productId", nameof(productId));
            if (quantity <= 0) throw new ArgumentException("Invalid quantity", nameof(quantity));
            if (price <= 0) throw new ArgumentException("Invalid price", nameof(price));
        }
    }
}
