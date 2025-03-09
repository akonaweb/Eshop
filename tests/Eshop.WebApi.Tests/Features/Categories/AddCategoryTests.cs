using Eshop.Domain;
using Eshop.WebApi.Features.Categories;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Categories
{
    public class AddCategoryTests : TestBase
    {
        [SetUp]
        public async Task Seed()
        {
            var category = await dbContext.Categories.AddAsync(new Category(0, "Category 1"));
            await dbContext.SaveChangesAsync(CancellationToken.None);
        }

        [Test]
        public async Task AddCategory_ReturnsCorrectDto()
        {
            // arrange
            var query = new AddCategory.Command(new AddCategoryRequestDto
            {
                Name = "Test category"
            });
            var handler = new AddCategory.Hanlder(dbContext);

            // act
            var sut = await handler.Handle(query, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }
    }
}
