using Eshop.Shared.Tests.Mocks;
using Eshop.Shared.Tests.Utils;

namespace Eshop.Domain.Tests
{
    public class CategoryTests
    {
        [Test]
        public void Category_WithValidParams_SetCorrectlyProperties()
        {
            // arrange
            var id = 0; // Zero is valid for a new object (EF)
            var name = StringUtils.GenerateRandomString(50);

            // act
            var sut = new Category(id, name);

            Assert.Multiple(() =>
            {
                // assert
                Assert.That(sut.Id, Is.EqualTo(id));
                Assert.That(sut.Name, Is.EqualTo(name));
            });
        }

        [Test]
        public void Category_WithInvalidIdParam_ThrowsException()
        {
            // act/assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Category(-1, "Category Name"));
        }

        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy")] // 51 chars
        public void Category_WithInvalidParams_ThrowsException(string? name)
        {
            // act/assert
            Assert.Throws<ArgumentNullException>(() => new Category(0, name!));
        }

        [Test]
        public void CategoryUpdate_WithValidParams_SetsPropertiesCorreclty()
        {
            // arrange
            var newCategoryName = StringUtils.GenerateRandomString(50);
            var sut = CategoryMocks.GetCategory1();

            // act
            sut.Udpate(newCategoryName);

            // assert
            Assert.That(sut.Name, Is.EqualTo(newCategoryName));
        }

        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy")] // 51 chars
        public void CategoryUpdate_WithInvalidParams_ThrowsException(string? newCategoryName)
        {
            // arrange
            var sut = CategoryMocks.GetCategory1();

            // act/assert
            Assert.Throws<ArgumentNullException>(() => sut.Udpate(newCategoryName!));
        }
    }
}