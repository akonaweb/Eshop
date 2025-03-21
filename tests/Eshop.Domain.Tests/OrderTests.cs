namespace Eshop.Domain.Tests
{
    public class OrderTests
    {
        [Test]
        public void Order_WithValidParams_SetCorrectlyProperties()
        {
            // arrange
            var id = 0;
            var orderDate = DateTime.Now.Ticks;
            var orderItems = "Order Items";
            var totalPrice = 100;
            var status = "Active";

            // act
            var sut = new Order(id, orderDate, orderItems, totalPrice, status);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(sut.Id, Is.EqualTo(id));
                Assert.That(sut.OrderDate, Is.EqualTo(orderDate));
                Assert.That(sut.OrderItems, Is.EqualTo(orderItems));
                Assert.That(sut.TotalPrice, Is.EqualTo(totalPrice));
                Assert.That(sut.Status, Is.EqualTo(status));
            });
        }

        [Test]
        public void Order_WithInvalidIdParam_ThrowsException()
        {
            // act/assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Order(-1, DateTime.Now.Ticks, "Order Items", 100, "Active"));
        }

        [TestCase(0, " ", 100, "Active", typeof(ArgumentNullException))]
        [TestCase(0, "Order Items", -1, "Active", typeof(ArgumentOutOfRangeException))]
        [TestCase(0, null, 100, "Active", typeof(ArgumentNullException))]
        [TestCase(0, "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ", 100, "Active", typeof(ArgumentOutOfRangeException))]
        [TestCase(-1, "Order Items", 100, "Active", typeof(ArgumentOutOfRangeException))]
        public void Order_WithInvalidParams_ThrowsException(long orderDate, string? orderItems, int totalPrice, string? status, Type expectedException)
        {
            // act/assert
            Assert.Throws(expectedException, () => new Order(0, orderDate, orderItems!, totalPrice, status!));
        }


        [Test]
        public void OrderUpdate_WithValidParams_SetCorrectlyProperties()
        {
            // arrange
            var newOrderDate = DateTime.Now.Ticks;
            var newOrderItems = "Updated Order Items";
            var newTotalPrice = 200;
            var newStatus = "Completed";
            var sut = new Order(0, DateTime.Now.Ticks, "Order Items", 100, "Active");

            // act
            sut.Update(newOrderDate, newOrderItems, newTotalPrice, newStatus);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(sut.OrderDate, Is.EqualTo(newOrderDate));
                Assert.That(sut.OrderItems, Is.EqualTo(newOrderItems));
                Assert.That(sut.TotalPrice, Is.EqualTo(newTotalPrice));
                Assert.That(sut.Status, Is.EqualTo(newStatus));
            });
        }

        [TestCase(-1, "Updated Order Items", 200, "Completed", typeof(ArgumentOutOfRangeException))]
        [TestCase(0, null, 200, "Completed", typeof(ArgumentNullException))]
        [TestCase(0, " ", 200, "Completed", typeof(ArgumentNullException))]
        [TestCase(0, "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ", 200, "Completed", typeof(ArgumentOutOfRangeException))]
        [TestCase(0, "Updated Order Items", -1, "Completed", typeof(ArgumentOutOfRangeException))]
        public void OrderUpdate_WithInvalidParams_ThrowsException(long orderDate, string? orderItems, int totalPrice, string? status, Type expectedException)
        {
            // arrange
            var sut = new Order(0, DateTime.Now.Ticks, "Order Items", 100, "Active");

            // act/assert
            Assert.Throws(expectedException, () => sut.Update(orderDate, orderItems!, totalPrice, status!));
        }

    }
}
