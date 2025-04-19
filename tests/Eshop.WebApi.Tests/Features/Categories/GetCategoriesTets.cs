using Eshop.Shared.Tests.Mocks;
using Eshop.WebApi.Features.Categories;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Categories
{
    public class GetCategoriesTets : TestBase
    {
        [Test]
        public async Task GetCategories_ReturnsCorrectDto()
        {
            // arrange
            var category1 = CategoryMocks.GetCategory1();
            var category2 = CategoryMocks.GetCategory2();
            await dbContext.Categories.AddRangeAsync(category1, category2);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var query = new GetCategories.Query();
            var handler = new GetCategories.Handler(dbContext);

            // act
            var sut = await handler.Handle(query, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }

    }
}
