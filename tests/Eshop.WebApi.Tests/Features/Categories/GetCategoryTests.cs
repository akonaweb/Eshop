using Eshop.Domain;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Categories;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Categories
{
    public class GetCategoryTests : TestBase
    {
        [SetUp]
        public async Task Seed()
        {
            var category = await dbContext.Categories.AddAsync(new Category(0, "Category 1"));
            await dbContext.SaveChangesAsync(CancellationToken.None);
        }

        [Test]
        public async Task GetCategory_ReturnsCorrectDto()
        {
            // arrange
            var query = new GetCategory.Query(1);
            var handler = new GetCategory.Handler(dbContext);

            // act
            var sut = await handler.Handle(query, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }

        [Test]
        public void GetCategory_ThrowsArgumentNotFoundException()
        {
            // act
            var query = new GetCategory.Query(2);
            var handler = new GetCategory.Handler(dbContext);

            // assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
