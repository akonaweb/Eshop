using Eshop.Domain;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Categories;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Categories
{
    public class GetCategoryTests : TestBase
    {
        [Test]
        public async Task GetCategory_ReturnsCorrectDto()
        {
            // arrange
            var category = new Category(0, "Category 1");
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var query = new GetCategory.Query(category.Id);
            var handler = new GetCategory.Handler(dbContext);

            // act
            var sut = await handler.Handle(query, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }

        [Test]
        public void GetCategory_WithInvalidCategoryId_ThrowsNotFoundException()
        {
            // act
            var query = new GetCategory.Query(1);
            var handler = new GetCategory.Handler(dbContext);

            // assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
