using Eshop.Shared.Tests.Mocks;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Orders;
using Snapper;
using static Eshop.WebApi.Features.Orders.UpdateOrderRequestDto;

namespace Eshop.WebApi.Tests.Features.Orders
{
    public class UpdateOrderTests : TestBase
    {
        [Test]
        public void UpdateOrder_WithInvalidOrderId_ThrowsNotFoundException()
        {
            // Arrange
            var requestDto = new UpdateOrderRequestDto
            {
                Customer = "Valid Customer",
                Address = "Valid Address",
                Items =
                [
                    new UpdateOrderRequestDto.UpdateOrderItemRequestDto
                    {
                        ProductId = 1,
                        Quantity = 1
                    }
                ]
            };

            var query = new UpdateOrder.Command(1, requestDto);
            var handler = new UpdateOrder.Handler(dbContext);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Test]
        public async Task UpdateOrder_WithInvalidProductId_ThrowsNotFoundException()
        {
            // arrange
            var order = OrderMocks.GetOrder1();
            await dbContext.Orders.AddAsync(order, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var command = new UpdateOrder.Command(1, new UpdateOrderRequestDto
            {
                Customer = "Customer",
                Address = "Address",
                Items =
                [
                    new UpdateOrderItemRequestDto
                    {
                        ProductId = 0,
                        Quantity = 1
                    }
                ]
            });
            var handler = new UpdateOrder.Handler(dbContext);

            // act/assert
            Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task UpdateOrder_WithValidData_UpdatesOrderSuccessfully()
        {
            // Arrange
            var order = OrderMocks.GetOrder1();
            dbContext.Orders.Add(order);
            dbContext.Products.Add(ProductMocks.GetProduct3());
            await dbContext.SaveChangesAsync();

            var requestDto = new UpdateOrderRequestDto
            {
                Customer = "Updated Customer",
                Address = "Updated Address",
                Items =
                [
                    new UpdateOrderRequestDto.UpdateOrderItemRequestDto
                    {
                        ProductId = 1,
                        Quantity = 2
                    },
                    new UpdateOrderRequestDto.UpdateOrderItemRequestDto
                    {
                        ProductId = 3,
                        Quantity = 3
                    }
                ]
            };

            var query = new UpdateOrder.Command(order.Id, requestDto);
            var handler = new UpdateOrder.Handler(dbContext);

            // Act  
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert  
            result.ShouldMatchSnapshot();
        }       
    }
}
