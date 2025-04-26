using Eshop.Shared.Tests.Mocks;
using Eshop.WebApi.Features.Orders;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Orders
{
    public class GetOrdersTests : TestBase
    {
        [Test]
        public async Task GetOrders_ReturnsCorrectDto()
        {
            // arrange
            var orderItem1 = OrderItemMocks.GetOrderItem1();
            var orderItem2 = OrderItemMocks.GetOrderItem2();
            var order = OrderMocks.GetOrder1();
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);
            await dbContext.Orders.AddAsync(order, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var query = new GetOrders.Query();
            var handler = new GetOrders.Handler(dbContext);

            // act
            var sut = await handler.Handle(query, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }
    }
}