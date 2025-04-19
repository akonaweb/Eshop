using Eshop.Shared.Tests.Mocks;
using Eshop.Shared.Tests.Utils;

namespace Eshop.Domain.Tests
{
    public class ProductTests
    {
        [Test]
        public void Product_WithValidParams_SetCorrectlyProperties()
        {
            // arrange
            var id = 0; // Zero is valid for a new object (EF)
            var title = StringUtils.GenerateRandomString(50);
            var description = StringUtils.GenerateRandomString(500);
            var price = 0; // Zero is allowed
            var category = CategoryMocks.GetCategory1();

            // act
            var sut = new Product(id, title, description, price, category);

            Assert.Multiple(() =>
            {
                // assert
                Assert.That(sut.Id, Is.EqualTo(id));
                Assert.That(sut.Title, Is.EqualTo(title));
                Assert.That(sut.Description, Is.EqualTo(description));
                Assert.That(sut.Price, Is.EqualTo(price));
                Assert.That(sut.Category, Is.EqualTo(category));
            });
        }

        [Test]
        public void Product_WithInvalidIdParam_ThrowsException()
        {
            // act/assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Product(-1, "Title", "Description", 1200, null));
        }

        // Title property
        [TestCase(null, "Description", 0)]
        [TestCase(" ", "Description", 0)]
        [TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy", "Description", 0)] // 51 chars
        // Description property
        [TestCase("Title", null, 0)]
        [TestCase("Title", " ", 0)]
        [TestCase("Title", "ZbrEmS4KqO8JzyA13tdYtvb5b3B7WFiIej0PdgMlnQyC2tnS5OfVsf0boGoPpJ8YmdnEhDVBoBuE1fjKDAHwhDZnBdxQNmRzUH0SRk44sBcu4yMO3pdu63UBdXiWi02Q7nFDaNRpx3Nyi9YGeRO2L48wR3dTbhuJDVz4KwhnV89K3WW8QXpL2dFdZz0CDRTWsxNWq48x6hsgQiX4IOvMob6qVIOLfHIrUT8kSCNevxqzTRynAKrwoxkCLcEgYq7u699PupVYTbP8es3X8YvtehAP8IYA89T1AfScBA0yrhgke4OKPR9HdG6crui42yN2aINCqhPqpMPKdyAVgU7XdQFD2UYSIvDAdBWS6XJk0ftiAkrVRWZC8nK5InU49ALqAOtwVxFeo3Eevszr9sMKcYtnYQtwc6RDHnhjCz8VWViyfludPkwFQ2yJqF6jHucTl9PNZXrhG03iwzzan22clBSqjcLd2l90fcHK10ZbxyYlwm2pHv8xi", 0)] // 501 chars
        // Price property
        [TestCase("Title", "Description", -1)]
        public void Product_WithInvalidParams_ThrowsException(string? title, string? description, decimal price)
        {
            // act/assert
            Assert.Throws<ArgumentNullException>(() => new Product(0, title!, description!, price, null));
        }

        [Test]
        public void ProductUpdate_WithValidParams_SetCorrectlyProperties()
        {
            // arrange
            var newTitle = StringUtils.GenerateRandomString(50);
            var newDescription = StringUtils.GenerateRandomString(500);
            var newPrice = new decimal(new Random().NextDouble());
            var newCategory = CategoryMocks.GetCategory2();
            var sut = ProductMocks.GetProduct1();

            // act
            sut.Update(newTitle, newDescription, newPrice, newCategory);

            Assert.Multiple(() =>
            {
                // assert
                Assert.That(sut.Title, Is.EqualTo(newTitle));
                Assert.That(sut.Description, Is.EqualTo(newDescription));
                Assert.That(sut.Price, Is.EqualTo(newPrice));
                Assert.That(sut.Category, Is.EqualTo(newCategory));
            });
        }


        // Title property
        [TestCase(null, "Description", 0)]
        [TestCase(" ", "Description", 0)]
        [TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy", "Description", 0)] // 51 chars
        // Description property
        [TestCase("Title", null, 0)]
        [TestCase("Title", " ", 0)]
        [TestCase("Title", "ZbrEmS4KqO8JzyA13tdYtvb5b3B7WFiIej0PdgMlnQyC2tnS5OfVsf0boGoPpJ8YmdnEhDVBoBuE1fjKDAHwhDZnBdxQNmRzUH0SRk44sBcu4yMO3pdu63UBdXiWi02Q7nFDaNRpx3Nyi9YGeRO2L48wR3dTbhuJDVz4KwhnV89K3WW8QXpL2dFdZz0CDRTWsxNWq48x6hsgQiX4IOvMob6qVIOLfHIrUT8kSCNevxqzTRynAKrwoxkCLcEgYq7u699PupVYTbP8es3X8YvtehAP8IYA89T1AfScBA0yrhgke4OKPR9HdG6crui42yN2aINCqhPqpMPKdyAVgU7XdQFD2UYSIvDAdBWS6XJk0ftiAkrVRWZC8nK5InU49ALqAOtwVxFeo3Eevszr9sMKcYtnYQtwc6RDHnhjCz8VWViyfludPkwFQ2yJqF6jHucTl9PNZXrhG03iwzzan22clBSqjcLd2l90fcHK10ZbxyYlwm2pHv8xi", 0)] // 501 chars
        // Price property
        [TestCase("Title", "Description", -1)]
        public void ProductUpdate_WithInvalidParams_ThrowsException(string? title, string? description, decimal price)
        {
            // arrange
            var sut = ProductMocks.GetProduct1();

            // act/assert
            Assert.Throws<ArgumentNullException>(() => sut.Update(title!, description!, price, null));
        }
    }
}