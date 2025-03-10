using Eshop.Domain;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Categories;
using Microsoft.EntityFrameworkCore;

namespace Eshop.WebApi.Tests.Features.Categories
{
    public class DeleteCategoryTests : TestBase
    {
        [Test]
        public async Task DeleteCategory_ReturnsCorrectDto()
        {
            // arrange
            var category = new Category(0, "Original Category");
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            var query = new DeleteCategory.Command(1);
            var handler = new DeleteCategory.Handler(dbContext);

            // act
            var sut = await handler.Handle(query, CancellationToken.None);

            // assert
            var categories = await dbContext.Categories.ToListAsync(CancellationToken.None);
            Assert.That(categories, Is.Empty);
        }

        [Test]
        public void DeleteCategory_ThrowsNotFoundException()
        {
            // arrange
            var query = new DeleteCategory.Command(2);
            var handler = new DeleteCategory.Handler(dbContext);

            // assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
