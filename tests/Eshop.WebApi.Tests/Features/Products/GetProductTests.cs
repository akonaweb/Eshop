using Eshop.Domain;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Products;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Products
{
    public class GetProductTests : TestBase
    {
        [SetUp]
        public async Task Seed()
        {
            var category = await dbContext.Categories.AddAsync(new Category(0, "Category 1"));
            await dbContext.Products.AddAsync(new Product(0, "Title", "Description", 1, category.Entity));
            await dbContext.SaveChangesAsync(CancellationToken.None);
        }

        [Test]
        public async Task GetProduct_ReturnsCorrectDto()
        {
            // arrange
            var query = new GetProduct.Query(1);
            var handler = new GetProduct.Handler(dbContext);

            // act
            var sut = await handler.Handle(query, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }

        [Test]
        public void GetProduct_ThrowsNotFoundException()
        {
            // act
            var query = new GetProduct.Query(2);
            var handler = new GetProduct.Handler(dbContext);

            // assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
