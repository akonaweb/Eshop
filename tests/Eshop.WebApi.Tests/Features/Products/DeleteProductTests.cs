using Eshop.Domain;
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
            var category = await dbContext.Categories.AddAsync(new Category(0, "Category 1"));
            await dbContext.Products.AddAsync(new Product(0, "Title", "Description", 1, category.Entity));
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var query = new DeleteProduct.Command(1);
            var handler = new DeleteProduct.Handler(dbContext);

            // act
            var sut = await handler.Handle(query, CancellationToken.None);

            // assert
            var product = await dbContext.Products.ToListAsync(CancellationToken.None);
            Assert.That(product, Is.Empty);
        }

        [Test]
        public void DeleteProduct_WithInvalidProductId_ThrowsNotFoundException()
        {
            // arrange
            var query = new DeleteProduct.Command(1);
            var handler = new DeleteProduct.Handler(dbContext);

            // act/assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
