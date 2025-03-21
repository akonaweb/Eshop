namespace Eshop.Domain
{
    public class Order
    {
        public int Id { get; }
        public long OrderDate { get; private set; }
        public string OrderItems { get; private set; }
        public int TotalPrice { get; private set; }
        public string Status { get; private set; }

        public Order(int id, long orderDate, string orderItems, int totalPrice, string status)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be a non-negative value.");

            if (orderDate < 0)
                throw new ArgumentOutOfRangeException(nameof(orderDate), "Order date must be a non-negative value.");

            if (string.IsNullOrWhiteSpace(orderItems))
                throw new ArgumentNullException(nameof(orderItems), "Order items cannot be null or empty.");

            if (orderItems.Length > 500)
                throw new ArgumentOutOfRangeException(nameof(orderItems), "Order items cannot exceed 500 characters.");

            if (totalPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(totalPrice), "Total price must be a non-negative value.");

            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentNullException(nameof(status), "Status cannot be null or empty.");

            Id = id;
            OrderDate = orderDate;
            OrderItems = orderItems;
            TotalPrice = totalPrice;
            Status = status;
        }

        public void Update(long orderDate, string orderItems, int totalPrice, string status)
        {
            if (orderDate < 0)
                throw new ArgumentOutOfRangeException(nameof(orderDate), "Order date must be a non-negative value.");

            if (string.IsNullOrWhiteSpace(orderItems))
                throw new ArgumentNullException(nameof(orderItems), "Order items cannot be null or empty.");

            if (orderItems.Length > 500)
                throw new ArgumentOutOfRangeException(nameof(orderItems), "Order items cannot exceed 500 characters.");

            if (totalPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(totalPrice), "Total price must be a non-negative value.");

            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentNullException(nameof(status), "Status cannot be null or empty.");

            OrderDate = orderDate;
            OrderItems = orderItems;
            TotalPrice = totalPrice;
            Status = status;
        }
    }

}
