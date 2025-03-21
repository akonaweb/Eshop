namespace Eshop.Domain.Tests
{
    public class OrderItemTests
    {
        [Test]
        public void OrderItem_WithValidParams_SetCorrectlyProperties()
        {
            // arrange
            var id = 0;
            var productId = 1;
            var quantity = 2;
            var price = 100;
            var orderId = 1;
            var order = new Order(0, DateTime.Now.Ticks, "Notebooks", 200, "New"); // Example Order setup

            // act
            var sut = new OrderItem(id, productId, quantity, price, orderId, order);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(sut.Id, Is.EqualTo(id));
                Assert.That(sut.ProductId, Is.EqualTo(productId));
                Assert.That(sut.Quantity, Is.EqualTo(quantity));
                Assert.That(sut.Price, Is.EqualTo(price));
                Assert.That(sut.OrderId, Is.EqualTo(orderId));
            });
        }

        [Test]
        public void OrderItem_WithInvalidIdParam_ThrowsException()
        {
            // arrange
            var productId = 1;
            var quantity = 2;
            var price = 100;
            var orderId = 1;
            var order = new Order(0, DateTime.Now.Ticks, "Notebooks", 200, "New");

            // act/assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new OrderItem(-1, productId, quantity, price, orderId, order));
        }

        [TestCase(0, 2, 100, 1)]
        [TestCase(1, -1, 100, 1)]
        [TestCase(1, 2, -100, 1)]
        public void OrderItem_WithInvalidParams_ThrowsException(int productId, int quantity, decimal price, int orderId)
        {
            // arrange
            var order = new Order(0, DateTime.Now.Ticks, "Notebooks", 200, "New");

            // act/assert
            Assert.Throws<ArgumentException>(() => new OrderItem(0, productId, quantity, price, orderId, order));
        }

        [Test]
        public void OrderItemUpdate_WithValidParams_SetCorrectlyProperties()
        {
            // arrange
            var newProductId = 2;
            var newQuantity = 3;
            var newPrice = 150;
            var orderId = 1;
            var order = new Order(0, DateTime.Now.Ticks, "Notebooks", 200, "New");
            var sut = new OrderItem(0, 1, 2, 100, orderId, order);

            // act
            sut.Update(newProductId, newQuantity, newPrice);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(sut.ProductId, Is.EqualTo(newProductId));
                Assert.That(sut.Quantity, Is.EqualTo(newQuantity));
                Assert.That(sut.Price, Is.EqualTo(newPrice));
            });
        }

        [TestCase(0, 2, 100)]
        [TestCase(1, -1, 100)]
        [TestCase(1, 2, -100)]
        public void OrderItemUpdate_WithInvalidParams_ThrowsException(int productId, int quantity, decimal price)
        {
            // arrange
            var order = new Order(0, DateTime.Now.Ticks, "Notebooks", 200, "New");
            var sut = new OrderItem(0, 1, 2, 100, 1, order);

            // act/assert
            Assert.Throws<ArgumentException>(() => sut.Update(productId, quantity, price));
        }
    }
}
