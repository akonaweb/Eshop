using Eshop.Domain;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Products;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Products
{
    public class UpdateProductTests : TestBase
    {
        [SetUp]
        public async Task Seed()
        {
            var category = await dbContext.Categories.AddAsync(new Category(0, "Category 1"));
            await dbContext.Products.AddAsync(new Product(0, "Test product", "bla", 1, category.Entity));
            await dbContext.SaveChangesAsync();
        }

        [Test]
        public void UpdateProduct_ThrowsArgumentNullException()
        {
            // act

            var requestDto = new UpdateProductRequestDto
            {
                Title = "Test product",
                Description = "bla",
                Price = 1,
                CategoryId = 1
            };
            var query = new UpdateProduct.Command(0, requestDto);
            var handler = new UpdateProduct.Handler(dbContext);
            // assert

            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
