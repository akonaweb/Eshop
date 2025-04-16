using Eshop.Domain;
using Eshop.WebApi.Features.Orders;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Orders
{
    public class GetCartTests : TestBase
    {
        [Test]
        public async Task GetCart_ReturnsCorrectDto()
        {
            // arrange
            var product1 = new Product(0, "Product 1", "Description 1", 10, null);
            var product2 = new Product(0, "Product 2", "Description 2", 20, null);

            await dbContext.Products.AddRangeAsync(product1, product2);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var query = new GetCart.Query(new List<GetCartRequestDto>
            {
                new GetCartRequestDto
                {
                    ProductId = 1,
                    Quantity = 2 
                },
                new GetCartRequestDto
                {
                    ProductId = 2,
                    Quantity = 3
                }
            });

            var handler = new GetCart.Handler(dbContext);

            // act
            var sut = await handler.Handle(query, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }
    }
}
