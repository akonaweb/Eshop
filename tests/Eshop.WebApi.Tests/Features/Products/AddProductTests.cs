using Eshop.Domain;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Products;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Products
{
    public class AddProductTests : TestBase
    {
        [Test]
        public async Task AddProduct_ReturnsCorrectDto()
        {
            // arrange
            await dbContext.Categories.AddAsync(new Category(0, "Category 1"));
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var query = new AddProduct.Command(new AddProductRequestDto
            {
                Title = "Title",
                Description = "Description",
                Price = 1,
                CategoryId = 1
            });
            var handler = new AddProduct.Handler(dbContext);

            // act
            var sut = await handler.Handle(query, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }

        [Test]
        public void AddProduct_WithInvalidCategoryId_ThrowsNotFoundException()
        {
            // arrange
            var query = new AddProduct.Command(new AddProductRequestDto
            {
                Title = "Title",
                Description = "Description",
                Price = 1,
                CategoryId = 1
            });

            var handler = new AddProduct.Handler(dbContext);

            // act/assert
            Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(query, CancellationToken.None));
        }
    }
}