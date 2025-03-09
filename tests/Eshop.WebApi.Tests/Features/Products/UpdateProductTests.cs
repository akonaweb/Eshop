using Eshop.Domain;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Products;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Products
{
    public class UpdateProductTests : TestBase
    {
        [Test]
        public async Task UpdateProduct_ChangeProperties()
        {
            // arrange
            var category = await dbContext.Categories.AddAsync(new Category(0, "Category 1"));
            var category2 = await dbContext.Categories.AddAsync(new Category(0, "Category 2"));
            await dbContext.Products.AddAsync(new Product(0, "Title", "Description", 1, category.Entity));
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var requestDto = new UpdateProductRequestDto
            {
                Title = "Title 2",
                Description = "Descritpion 2",
                Price = 2,
                CategoryId = 2
            };
            var query = new UpdateProduct.Command(1, requestDto);
            var handler = new UpdateProduct.Handler(dbContext);

            /// act
            var sut = await handler.Handle(query, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }

        [Test]
        public void UpdateProduct_WithInvalidProductId_ThrowsNotFoundException()
        {
            // arrange
            var requestDto = new UpdateProductRequestDto
            {
                Title = "Title",
                Description = "Descritpion",
                Price = 1,
            };
            var query = new UpdateProduct.Command(1, requestDto);
            var handler = new UpdateProduct.Handler(dbContext);

            // act/assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Test]
        public async Task UpdateProduct_WithInvalidCategoryId_ThrowsNotFoundException()
        {
            // arrange
            var category = await dbContext.Categories.AddAsync(new Category(0, "Category 1"));
            await dbContext.Products.AddAsync(new Product(0, "Title", "Description", 1, category.Entity));
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var requestDto = new UpdateProductRequestDto
            {
                Title = "Updated Title",
                Description = "Updated Description",
                Price = 2,
                CategoryId = 2
            };

            var query = new UpdateProduct.Command(1, requestDto);
            var handler = new UpdateProduct.Handler(dbContext);

            // act/assert
            Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(query, CancellationToken.None));
        }
    }
}