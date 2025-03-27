namespace Eshop.Domain.Tests
{
    public class OrderTests
    {
        [Test]
        public void Order_WithValidParams_SetsCorrectlyProperties()
        {
            // arrange
            var id = 1;
            var createdAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var orderItems = new List<OrderItem>();
            var name = "John Doe";
            var street = "123 Main St";
            var city = "Anytown";
            var zipCode = 12345;

            // act
            var sut = new Order(id, createdAt, orderItems, name, street, city, zipCode);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(sut.Id, Is.EqualTo(id));
                Assert.That(sut.createdAt, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(createdAt).DateTime));
                Assert.That(sut.OrderItems, Is.EqualTo(orderItems));
                Assert.That(sut.Name, Is.EqualTo(name));
                Assert.That(sut.Street, Is.EqualTo(street));
                Assert.That(sut.City, Is.EqualTo(city));
                Assert.That(sut.ZipCode, Is.EqualTo(zipCode));
            });
        }

        [Test]
        public void Order_WithNegativeId_ThrowsArgumentOutOfRangeException()
        {
            // arrange
            var createdAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var orderItems = new List<OrderItem>();
            var name = "John Doe";
            var street = "123 Main St";
            var city = "Anytown";
            var zipCode = 12345;

            // act/assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Order(-1, createdAt, orderItems, name, street, city, zipCode));
        }

        [Test]
        public void Order_WithNegativeOrderDate_ThrowsArgumentOutOfRangeException()
        {
            // arrange
            var orderItems = new List<OrderItem>();
            var name = "John Doe";
            var street = "123 Main St";
            var city = "Anytown";
            var zipCode = 12345;

            // act/assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Order(1, -100, orderItems, name, street, city, zipCode));
        }

        [Test]
        public void Order_WithNullOrderItems_ThrowsArgumentNullException()
        {
            // arrange
            var createdAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var name = "John Doe";
            var street = "123 Main St";
            var city = "Anytown";
            var zipCode = 12345;

            // act/assert
            Assert.Throws<ArgumentNullException>(() => new Order(1, createdAt, null!, name, street, city, zipCode));
        }

        [Test]
        public void Order_WithInvalidZipCode_ThrowsArgumentOutOfRangeException()
        {
            // arrange
            var createdAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var orderItems = new List<OrderItem>();
            var name = "John Doe";
            var street = "123 Main St";
            var city = "Anytown";

            // act/assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Order(1, createdAt, orderItems, name, street, city, -1));
        }

        [Test]
        public void OrderUpdate_WithValidParams_UpdatesPropertiesCorrectly()
        {
            // arrange
            var order = new Order(1, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), new List<OrderItem>(), "John Doe", "123 Main St", "Anytown", 12345);
            var newCreatedAt = DateTime.UtcNow;
            var newOrderItems = new List<OrderItem> { new OrderItem(1, new Product(1, "Laptop", "Gaming Laptop", 1500, null), 2, 1500, 1, 1) };
            var newName = "Jane Doe";
            var newStreet = "456 Elm St";
            var newCity = "Othertown";
            var newZipCode = 67890;

            // act
            order.Update(newCreatedAt, newOrderItems, newName, newStreet, newCity, newZipCode);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(order.createdAt, Is.EqualTo(newCreatedAt).Within(TimeSpan.FromSeconds(1)));
                Assert.That(order.OrderItems, Is.EqualTo(newOrderItems));
                Assert.That(order.Name, Is.EqualTo(newName));
                Assert.That(order.Street, Is.EqualTo(newStreet));
                Assert.That(order.City, Is.EqualTo(newCity));
                Assert.That(order.ZipCode, Is.EqualTo(newZipCode));
            });
        }

        [Test]
        public void OrderUpdate_WithDefaultDate_ThrowsArgumentOutOfRangeException()
        {
            // arrange
            var order = new Order(1, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), new List<OrderItem>(), "John Doe", "123 Main St", "Anytown", 12345);

            // act/assert
            Assert.Throws<ArgumentOutOfRangeException>(() => order.Update(default, new List<OrderItem>(), "Jane Doe", "456 Elm St", "Othertown", 67890));
        }

        [Test]
        public void OrderUpdate_WithNullOrderItems_ThrowsArgumentNullException()
        {
            // arrange
            var order = new Order(1, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), new List<OrderItem>(), "John Doe", "123 Main St", "Anytown", 12345);

            // act/assert
            Assert.Throws<ArgumentNullException>(() => order.Update(DateTime.UtcNow, null!, "Jane Doe", "456 Elm St", "Othertown", 67890));
        }
    }
}
