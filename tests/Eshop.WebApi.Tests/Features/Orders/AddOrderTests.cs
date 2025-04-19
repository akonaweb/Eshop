using Eshop.Shared.Tests.Mocks;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Orders;
using Snapper;
using static Eshop.WebApi.Features.Orders.AddOrderRequestDto;

namespace Eshop.WebApi.Tests.Features.Orders
{
    public class AddOrderTests : TestBase
    {
        [Test]
        public async Task AddOrder_ReturnsCorrectDto()
        {
            // arrange
            var product1 = ProductMocks.GetProduct1();
            var product2 = ProductMocks.GetProduct2();

            await dbContext.Products.AddRangeAsync(product1, product2);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var command = new AddOrder.Command(new AddOrderRequestDto
            {
                Customer = "Customer",
                Address = "Address",
                Items = new List<AddOrderItemRequestDto>
                {
                    new AddOrderItemRequestDto
                    {
                        ProductId = product1.Id,
                        Quantity = 2
                    },
                    new AddOrderItemRequestDto
                    {
                        ProductId = product2.Id,
                        Quantity = 3
                    }
                }
            });

            var handler = new AddOrder.Hanlder(dbContext);

            // act
            var sut = await handler.Handle(command, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }

        [Test]
        public void AddOrder_WithInvalidProductId_ThrowsNotFoundException()
        {
            // arrange
            var query = new AddOrder.Command(new AddOrderRequestDto
            {
                Customer = "Customer",
                Address = "Address",
                Items = new List<AddOrderItemRequestDto>
                    {
                        new AddOrderItemRequestDto
                        {
                            ProductId = 0,
                            Quantity = 1
                        }
                    }
            });
            var handler = new AddOrder.Hanlder(dbContext);

            // act/assert
            Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(query, CancellationToken.None));
        }
    }
}