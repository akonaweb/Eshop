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
            await dbContext.Products.AddAsync(new Product(0, "Title", "Description", 1, null));
            await dbContext.SaveChangesAsync(CancellationToken.None);
        }

        [Test]
        public void UpdateProduct_ThrowsArgumentNotFoundException()
        {
            // act
            var requestDto = new UpdateProductRequestDto
            {
                Title = "Title",
                Description = "Descritpion",
                Price = 1,
            };
            var query = new UpdateProduct.Command(0, requestDto);
            var handler = new UpdateProduct.Handler(dbContext);

            // assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Test]
        public async Task UpdateProduct_ChangeProperties()
        {
            // act
            var requestDto = new UpdateProductRequestDto
            {
                Title = "Title 2",
                Description = "Descritpion 2",
                Price = 2,
                CategoryId = 1
            };
            var query = new UpdateProduct.Command(1, requestDto);
            var handler = new UpdateProduct.Handler(dbContext);

            /// act
            var sut = await handler.Handle(query, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }
    }
}