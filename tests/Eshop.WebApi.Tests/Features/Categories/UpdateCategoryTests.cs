using Eshop.Shared.Tests.Mocks;
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
            var category = await dbContext.Categories.AddAsync(CategoryMocks.GetCategory1());
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var requestDto = new UpdateCategoryRequestDto { Name = "Category 2" };
            var command = new UpdateCategory.Command(category.Entity.Id, requestDto);
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
            var command = new UpdateCategory.Command(1, requestDto);
            var handler = new UpdateCategory.Handler(dbContext);

            // act/assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
