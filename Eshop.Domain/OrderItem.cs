using System.Diagnostics.CodeAnalysis;

namespace Eshop.Domain
{
    public class OrderItem
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        [ExcludeFromCodeCoverage]
        private OrderItem() { } // private ctor needed for a persistence - Entity Framework
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public OrderItem(int quantity, decimal price, Product product)
        {
            ValidateParameters(quantity, price, product);

            Quantity = quantity;
            Price = price;
            Product = product;
        }

        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        public Product Product { get; private set; }
        public decimal TotalPrice => Quantity * Price;

        private static void ValidateParameters(int quantity, decimal price, Product product)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegative(price);

            if (product is null)
                throw new ArgumentNullException(nameof(product));
        }
    }
}
