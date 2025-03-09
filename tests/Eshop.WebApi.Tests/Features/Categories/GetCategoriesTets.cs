using Eshop.Domain;
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
            var category = new Category(0, "Category 1");
            await dbContext.Categories.AddAsync(category);
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
