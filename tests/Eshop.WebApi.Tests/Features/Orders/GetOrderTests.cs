using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Orders;
using Eshop.Domain;
using Snapper;
using Microsoft.EntityFrameworkCore;

namespace Eshop.WebApi.Tests.Features.Orders
{
    public class GetOrderTests : TestBase
    {
        [Test]
        public async Task GetOrder_ReturnsCorrectDto()
        {
            // arrange
            var order = new Order(0, "Customer", "Address");
            order.AddItem(new OrderItem(1, 1, new Product(0, "Product 1", "Description", 1, null)));
            await dbContext.Orders.AnyAsync(CancellationToken.None);
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
