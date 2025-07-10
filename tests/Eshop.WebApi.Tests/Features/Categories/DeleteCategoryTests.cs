using Eshop.Shared.Tests.Mocks;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Categories;
using Microsoft.EntityFrameworkCore;

namespace Eshop.WebApi.Tests.Features.Categories
{
    public class DeleteCategoryTests : TestBase
    {
        [Test]
        public async Task DeleteCategory_RemovesCategoryFromDb()
        {
            // arrange
            var category = await dbContext.Categories.AddAsync(CategoryMocks.GetCategory1());
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var command = new DeleteCategory.Command(category.Entity.Id);
            var handler = new DeleteCategory.Handler(dbContext);

            // act
            var sut = await handler.Handle(command, CancellationToken.None);

            // assert
            var result = await dbContext.Categories.ToListAsync(CancellationToken.None);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void DeleteCategory_WithInvalidCategoryId_ThrowsNotFoundException()
        {
            // arrange
            var command = new DeleteCategory.Command(1);
            var handler = new DeleteCategory.Handler(dbContext);

            // assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
