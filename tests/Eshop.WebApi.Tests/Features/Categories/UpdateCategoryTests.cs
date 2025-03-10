using Eshop.Domain;
using Eshop.WebApi.Features.Categories;
using Snapper;
using Eshop.WebApi.Exceptions;

namespace Eshop.WebApi.Tests.Features.Categories
{
    public class UpdateCategoryTests : TestBase
    {
        [Test]
        public async Task UpdateCategory_ReturnsCorrectDto()
        {
            // arrange
            var category = new Category(0, "Original Category");
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            var requestDto = new UpdateCategoryRequestDto { Name = "Category 2" };
            var command = new UpdateCategory.Command(1, requestDto);
            var handler = new UpdateCategory.Handler(dbContext);

            // act
            var sut = await handler.Handle(command, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }

        [Test]
        public async Task UpdateCategory_ChangeProperties()
        {
            // arrange
            var category = new Category(0, "Original Category");
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            var requestDto = new UpdateCategoryRequestDto { Name = "Category 2" };
            var command = new UpdateCategory.Command(1, requestDto);
            var handler = new UpdateCategory.Handler(dbContext);

            // act
            var sut = await handler.Handle(command, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }
    }
}
