using Eshop.Domain;
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
            await dbContext.Products.AddAsync(new Product(0, "Product 1", "Description", 1, null));
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var query = new AddOrder.Command(new AddOrderRequestDto
            {
                Customer = "Customer",
                Address = "Address",
                Items = new List<AddOrderItemRequestDto>
                {
                    new AddOrderItemRequestDto
                    {
                        ProductId = 1,
                        Quantity = 1
                    }
                }
            });
            var handler = new AddOrder.Hanlder(dbContext);

            // act
            var sut = await handler.Handle(query, CancellationToken.None);

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