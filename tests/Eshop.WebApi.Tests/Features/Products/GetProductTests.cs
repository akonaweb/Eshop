using Eshop.Shared.Tests.Mocks;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Products;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Products
{
    public class GetProductTests : TestBase
    {
        [Test]
        public async Task GetProduct_ReturnsCorrectDto()
        {
            // arrange
            var product = await dbContext.Products.AddAsync(ProductMocks.GetProduct1());
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var query = new GetProduct.Query(product.Entity.Id);
            var handler = new GetProduct.Handler(dbContext);

            // act
            var sut = await handler.Handle(query, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }

        [Test]
        public void GetProduct_WithInvalidProductId_ThrowsNotFoundException()
        {
            // arrange
            var query = new GetProduct.Query(1);
            var handler = new GetProduct.Handler(dbContext);

            // act/assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
