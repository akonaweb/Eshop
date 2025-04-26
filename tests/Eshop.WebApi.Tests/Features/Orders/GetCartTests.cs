using Eshop.Shared.Tests.Mocks;
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
            var product1 = ProductMocks.GetProduct1();
            var product2 = ProductMocks.GetProduct2();

            await dbContext.Products.AddRangeAsync(product1, product2);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var query = new GetCart.Query(new List<GetCartRequestDto>
            {
                new GetCartRequestDto
                {
                    ProductId = product1.Id,
                    Quantity = 2
                },
                new GetCartRequestDto
                {
                    ProductId = product2.Id,
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
