using Eshop.Domain;
using Eshop.WebApi.Features.Products;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Products
{
    public class GetProductsTests : TestBase
    {
        [Test]
        public async Task GetProducts_ReturnsCorrectDto()
        {
            // arrange
            var category = await dbContext.Categories.AddAsync(new Category(0, "Category 1"));
            await dbContext.Products.AddAsync(new Product(0, "Title", "Description", 1, category.Entity));
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var query = new GetProducts.Query();
            var handler = new GetProducts.Handler(dbContext);

            // act
            var sut = await handler.Handle(query, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }
    }
}
