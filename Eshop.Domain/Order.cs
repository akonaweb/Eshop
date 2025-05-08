using System.Diagnostics.CodeAnalysis;

namespace Eshop.Domain
{
    public class Order
    {
        private readonly List<OrderItem> items;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        [ExcludeFromCodeCoverage]
        private Order() { } // private ctor needed for a persistence - Entity Framework
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public Order(int id, string customer, string address, DateTime createdAt, IReadOnlyCollection<OrderItem> orderItems)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(id);

            ValidateParameters(customer, address, orderItems);

            Id = id;
            CreatedAt = createdAt;
            Customer = customer;
            Address = address;
            items = [.. orderItems];
        }

        public int Id { get; }
        public DateTime CreatedAt { get; }
        public string Customer { get; private set; }
        public string Address { get; private set; }
        public IReadOnlyCollection<OrderItem> Items => items;
        public decimal TotalPrice => Items.Sum(x => x.TotalPrice);

        public void Update(string customer, string address, IReadOnlyCollection<OrderItem> orderItems)
        {
            ValidateParameters(customer, address, orderItems);

            Customer = customer;
            Address = address;
            items.Clear();
            items.AddRange(orderItems);
        }

        private static void ValidateParameters(string customerName, string address, IReadOnlyCollection<OrderItem> orderItems)
        {
            if (string.IsNullOrEmpty(customerName?.Trim()) || customerName.Length > 50)
                throw new ArgumentNullException(nameof(customerName));

            if (string.IsNullOrEmpty(address?.Trim()) || address.Length > 500)
                throw new ArgumentNullException(nameof(address));

            var uniqueProductIdsCount = orderItems.Select(x => x.Product.Id).Distinct().Count();
            if (uniqueProductIdsCount != orderItems.Count)
                throw new InvalidOperationException(nameof(orderItems));
        }
    }
}
