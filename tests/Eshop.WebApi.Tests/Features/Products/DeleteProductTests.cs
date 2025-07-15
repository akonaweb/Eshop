using Eshop.Domain;
using Eshop.Shared.Tests.Mocks;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Products;
using Microsoft.EntityFrameworkCore;

namespace Eshop.WebApi.Tests.Features.Products
{
    public class DeleteProductTests : TestBase
    {
        [Test]
        public async Task DeleteProduct_RemovesProductFromDb()
        {
            // arrange
            var product = await dbContext.Products.AddAsync(ProductMocks.GetProduct1());
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var command = new DeleteProduct.Command(product.Entity.Id);
            var handler = new DeleteProduct.Handler(dbContext);

            // act
            var sut = await handler.Handle(command, CancellationToken.None);

            // assert
            var result = await dbContext.Products.ToListAsync(CancellationToken.None);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void DeleteProduct_WithInvalidProductId_ThrowsNotFoundException()
        {
            // arrange
            var command = new DeleteProduct.Command(1);
            var handler = new DeleteProduct.Handler(dbContext);

            // act/assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
