using Eshop.Domain;
using Eshop.WebApi.Features.Categories;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Categories
{
    public class AddCategoryTests : TestBase
    {
        [Test]
        public async Task AddCategory_ReturnsCorrectDto()
        {
            // arrange
            var category = new Category(0, "Original Category");
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

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
