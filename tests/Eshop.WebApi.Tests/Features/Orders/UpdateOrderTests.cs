using Eshop.Shared.Tests.Mocks;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Orders;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Orders
{
    public class UpdateOrderTests : TestBase
    {
        [Test]
        public async Task UpdateOrder_ChangeProperties()
        {
            // act
            var orderEntity = await dbContext.Orders.AddAsync(OrderMocks.GetOrder1());
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var order = orderEntity.Entity; // Fix for CS1061: Access the actual entity instead of the EntityEntry
            var requestDto = new UpdateOrdersRequestDto { Customer = "Jozef", Address = "123" };
            var command = new UpdateOrder.Command(order.Id, requestDto.Customer, requestDto.Address); // Fix for CS7036: Pass all required parameters
            var handler = new UpdateOrder.Handler(dbContext);

            /// act
            var sut = await handler.Handle(command, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }

        [Test]
        public void UpdateOrder_WithInvalidOrderId_ThrowsNotFoundException()
        {
            // arrange
            var requestDto = new UpdateOrdersRequestDto { Customer = "Jozef", Address = "123" };
            var query = new UpdateOrder.Command(1, requestDto.Customer, requestDto.Address); // Fix for CS7036: Pass all required parameters
            var handler = new UpdateOrder.Handler(dbContext);

            // act/assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
