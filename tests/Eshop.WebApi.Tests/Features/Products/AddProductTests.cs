using Eshop.Domain;
using Eshop.WebApi.Features.Products;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Products
{
    public class AddProductTests : TestBase
    {
        [SetUp]
        public async Task Seed()
        {
            var category = await dbContext.Categories.AddAsync(new Category(0, "Category 1"));
            await dbContext.SaveChangesAsync(CancellationToken.None);
        }

        [Test]
        public async Task AddProducts_ReturnsCorrectDto()
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

            // act
            var sut = await handler.Handle(query, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }
    }
}