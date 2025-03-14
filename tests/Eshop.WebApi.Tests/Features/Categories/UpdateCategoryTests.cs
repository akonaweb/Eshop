using Eshop.Domain;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Categories;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Categories
{
    public class UpdateCategoryTests : TestBase
    {
        [Test]
        public async Task UpdateCategory_ChangeProperties()
        {
            // act
            var category = new Category(0, "Category 1");
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var requestDto = new UpdateCategoryRequestDto { Name = "Category 2" };
            var command = new UpdateCategory.Command(1, requestDto);
            var handler = new UpdateCategory.Handler(dbContext);

            /// act
            var sut = await handler.Handle(command, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }

        [Test]
        public void UpdateCategory_WithInvalidCategoryId_ThrowsNotFoundException()
        {
            // arrange
            var requestDto = new UpdateCategoryRequestDto { Name = "Category 2" };
            var query = new UpdateCategory.Command(1, requestDto);
            var handler = new UpdateCategory.Handler(dbContext);

            // act/assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
