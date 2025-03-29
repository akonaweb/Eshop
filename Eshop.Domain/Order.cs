using System.Diagnostics.CodeAnalysis;

namespace Eshop.Domain
{
    public class Order
    {
        private readonly List<OrderItem> items = [];

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        [ExcludeFromCodeCoverage]
        private Order() { } // private ctor needed for a persistence - Entity Framework
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public Order(int id, string customer, string address)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(id);

            ValidateParameters(customer, address);

            Id = id;
            CreatedAt = DateTime.UtcNow; // Eventually we should use DateTime provider - we will have problem to test this prop
            Customer = customer;
            Address = address;
        }

        public int Id { get; }
        public DateTime CreatedAt { get; }
        public string Customer { get; }
        public string Address { get; }
        public IReadOnlyCollection<OrderItem> Items => items;
        public decimal TotalPrice => Items.Sum(x => x.TotalPrice);

        public void AddItem(OrderItem item)
        {
            if (items.Any(x => x.Product.Id == item.Product.Id))
                throw new InvalidOperationException($"Product with Id[{item.Product.Id}] already exists in the order.");

            items.Add(item);
        }

        private static void ValidateParameters(string customerName, string address)
        {
            if (string.IsNullOrEmpty(customerName?.Trim()) || customerName.Length > 50)
                throw new ArgumentNullException(nameof(customerName));

            if (string.IsNullOrEmpty(address?.Trim()) || address.Length > 500)
                throw new ArgumentNullException(nameof(address));
        }
    }
}
