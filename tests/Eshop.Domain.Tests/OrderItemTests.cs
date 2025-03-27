namespace Eshop.Domain.Tests
{
    public class OrderItemTests
    {
        [Test]
        public void OrderItem_WithValidParams_SetsCorrectlyProperties()
        {
            // arrange
            var id = 0;
            var product = new Product(1, "Laptop", "High-end laptop", 500, null);
            var quantity = 2;
            var price = 100;
            var orderId = 1;
            var productId = product.Id;

            // act
            var sut = new OrderItem(id, product, quantity, price, orderId, productId);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(sut.Id, Is.EqualTo(id));
                Assert.That(sut.Product, Is.EqualTo(product));
                Assert.That(sut.Quantity, Is.EqualTo(quantity));
                Assert.That(sut.Price, Is.EqualTo(price));
                Assert.That(sut.OrderId, Is.EqualTo(orderId));
                Assert.That(sut.ProductId, Is.EqualTo(productId));
            });
        }

        [Test]
        public void OrderItem_WithInvalidId_ThrowsArgumentOutOfRangeException()
        {
            // arrange
            var product = new Product(1, "Laptop", "High-end laptop", 500, null);
            var quantity = 2;
            var price = 100;
            var orderId = 1;
            var productId = product.Id;

            // act/assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new OrderItem(-1, product, quantity, price, orderId, productId));
        }

        [Test]
        public void OrderItem_WithNullProduct_ThrowsArgumentNullException()
        {
            // arrange
            var quantity = 2;
            var price = 100;
            var orderId = 1;
            var productId = 0;

            // act/assert
            Assert.Throws<ArgumentNullException>(() => new OrderItem(0, null!, quantity, price, orderId, productId));
        }

        [Test]
        public void OrderItem_WithInvalidQuantity_ThrowsArgumentException()
        {
            // arrange
            var product = new Product(1, "Laptop", "High-end laptop", 500, null);
            var price = 100;
            var orderId = 1;
            var productId = product.Id;

            // act/assert
            Assert.Throws<ArgumentException>(() => new OrderItem(0, product, -1, price, orderId, productId));
        }

        [Test]
        public void OrderItem_WithInvalidPrice_ThrowsArgumentException()
        {
            // arrange
            var product = new Product(1, "Laptop", "High-end laptop", 500, null);
            var quantity = 2;
            var orderId = 1;
            var productId = product.Id;

            // act/assert
            Assert.Throws<ArgumentException>(() => new OrderItem(0, product, quantity, -100, orderId, productId));
        }

        [Test]
        public void OrderItemUpdate_WithValidParams_UpdatesPropertiesCorrectly()
        {
            // arrange
            var product = new Product(1, "Laptop", "High-end laptop", 500, null);
            var newProduct = new Product(2, "Tablet", "Latest tablet", 300, null);
            var orderId = 1;
            var sut = new OrderItem(0, product, 2, 100, orderId, product.Id);

            // act
            sut.Update(newProduct, 3, 150);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(sut.Product, Is.EqualTo(newProduct));
                Assert.That(sut.Quantity, Is.EqualTo(3));
                Assert.That(sut.Price, Is.EqualTo(150));
                Assert.That(sut.ProductId, Is.EqualTo(newProduct.Id));
            });
        }

        [Test]
        public void OrderItemUpdate_WithNullProduct_ThrowsArgumentNullException()
        {
            // arrange
            var product = new Product(1, "Laptop", "High-end laptop", 500, null);
            var sut = new OrderItem(0, product, 2, 100, 1, product.Id);

            // act/assert
            Assert.Throws<ArgumentNullException>(() => sut.Update(null!, 2, 100));
        }

        [Test]
        public void OrderItemUpdate_WithInvalidQuantity_ThrowsArgumentException()
        {
            // arrange
            var product = new Product(1, "Laptop", "High-end laptop", 500, null);
            var newProduct = new Product(2, "Tablet", "Latest tablet", 300, null);
            var sut = new OrderItem(0, product, 2, 100, 1, product.Id);

            // act/assert
            Assert.Throws<ArgumentException>(() => sut.Update(newProduct, -1, 100));
        }

        [Test]
        public void OrderItemUpdate_WithInvalidPrice_ThrowsArgumentException()
        {
            // arrange
            var product = new Product(1, "Laptop", "High-end laptop", 500, null);
            var newProduct = new Product(2, "Tablet", "Latest tablet", 300, null);
            var sut = new OrderItem(0, product, 2, 100, 1, product.Id);

            // act/assert
            Assert.Throws<ArgumentException>(() => sut.Update(newProduct, 2, -100));
        }
    }
}
