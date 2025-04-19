using Eshop.Shared.Tests.Mocks;
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
            await dbContext.Products.AddRangeAsync(ProductMocks.GetProduct1(), ProductMocks.GetProduct2());
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
