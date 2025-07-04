using Eshop.Shared.Tests.Mocks;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Orders;
using Microsoft.EntityFrameworkCore;

namespace Eshop.WebApi.Tests.Features.Orders
{
    public class DeleteOrderTests : TestBase
    {
        [Test]
        public async Task DeleteOrder_RemovesOrderFromDb()
        {
            // arrange
            var order = await dbContext.Orders.AddAsync(OrderMocks.GetOrder1());
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var command = new DeleteOrder.Command(order.Entity.Id);
            var handler = new DeleteOrder.Handler(dbContext);

            // act
            var sut = await handler.Handle(command, CancellationToken.None);

            // assert
            var result = await dbContext.Orders.ToListAsync(CancellationToken.None);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void DeleteOrder_WithInvalidOrderId_ThrowsNotFoundException()
        {
            // arrange
            var command = new DeleteOrder.Command(1);
            var handler = new DeleteOrder.Handler(dbContext);

            // assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
