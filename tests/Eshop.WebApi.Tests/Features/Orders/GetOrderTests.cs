using Eshop.Shared.Tests.Mocks;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Orders;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Orders
{
    public class GetOrderTests : TestBase
    {
        [Test]
        public async Task GetOrder_ReturnsCorrectDto()
        {
            // arrange
            var order = OrderMocks.GetOrder1();
            await dbContext.Orders.AddAsync(order, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var query = new GetOrder.Query(order.Id);
            var handler = new GetOrder.Handler(dbContext);

            // act
            var sut = await handler.Handle(query, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }


        [Test]
        public void GetOrder_WithInvalidId_ThrowsNotFoundException()
        {
            // arrange
            var query = new GetOrder.Query(1);
            var handler = new GetOrder.Handler(dbContext);

            // act/assert
            Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(query, CancellationToken.None));
        }
    }
}
