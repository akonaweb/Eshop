using Eshop.Shared.Tests.Mocks;
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
            var category2 = await dbContext.Categories.AddAsync(CategoryMocks.GetCategory2());
            var product = await dbContext.Products.AddAsync(ProductMocks.GetProduct1());
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var requestDto = new UpdateProductRequestDto
            {
                Title = "Title 2",
                Description = "Descritpion 2",
                Price = 2,
                CategoryId = category2.Entity.Id
            };
            var command = new UpdateProduct.Command(product.Entity.Id, requestDto);
            var handler = new UpdateProduct.Handler(dbContext);

            /// act
            var sut = await handler.Handle(command, CancellationToken.None);

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
            var command = new UpdateProduct.Command(1, requestDto);
            var handler = new UpdateProduct.Handler(dbContext);

            // act/assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task UpdateProduct_WithInvalidCategoryId_ThrowsNotFoundException()
        {
            // arrange
            var product = await dbContext.Products.AddAsync(ProductMocks.GetProduct1());
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var requestDto = new UpdateProductRequestDto
            {
                Title = "Title",
                Description = "Description",
                Price = 1,
                CategoryId = 2
            };

            var command = new UpdateProduct.Command(product.Entity.Id, requestDto);
            var handler = new UpdateProduct.Handler(dbContext);

            // act/assert
            Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}