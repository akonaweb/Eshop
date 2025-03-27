using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Domain
{
    public class Order
    {
        public int Id { get; }
        public DateTime createdAt { get; }

        public List<OrderItem> OrderItems { get; private set; } = new();

        public string Name { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public int ZipCode { get; private set; }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public Order() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public Order(int id, long orderDate, List<OrderItem> orderItems, string name, string street, string city, int zipCode)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(id);

            if (orderDate < 0)
                throw new ArgumentOutOfRangeException(nameof(orderDate), "Order date must be a non-negative value.");

            if (orderItems == null)
                throw new ArgumentNullException(nameof(orderItems), "Order items cannot be null.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentNullException(nameof(street), "Street cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentNullException(nameof(city), "City cannot be null or empty.");

            if (zipCode <= 0)
                throw new ArgumentOutOfRangeException(nameof(zipCode), "Zip code must be a positive value.");

            Id = id;
            createdAt = DateTimeOffset.FromUnixTimeSeconds(orderDate).DateTime;
            OrderItems = orderItems;
            Name = name;
            Street = street;
            City = city;
            ZipCode = zipCode;
        }

        public void Update(DateTime createdAt, List<OrderItem> orderItems, string name, string street, string city, int zipCode)
        {
            if (createdAt == default)
                throw new ArgumentOutOfRangeException(nameof(createdAt), "Order date must be a valid date.");

            if (orderItems == null)
                throw new ArgumentNullException(nameof(orderItems), "Order items cannot be null.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentNullException(nameof(street), "Street cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentNullException(nameof(city), "City cannot be null or empty.");
            if (zipCode <= 0)
                throw new ArgumentOutOfRangeException(nameof(zipCode), "Zip code must be a positive value.");

            createdAt = createdAt;
            OrderItems = orderItems;
            Name = name;
            Street = street;
            City = city;
            ZipCode = zipCode;
        }
    }

}
