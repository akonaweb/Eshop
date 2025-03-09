﻿using Eshop.WebApi.Features.Categories;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Categories
{
    public class AddCategoryTests : TestBase
    {
        [Test]
        public async Task AddCategory_ReturnsCorrectDto()
        {
            // arrange
            var query = new AddCategory.Command(new AddCategoryRequestDto
            {
                Name = "Category 1"
            });
            var handler = new AddCategory.Hanlder(dbContext);

            // act
            var sut = await handler.Handle(query, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }
    }
}
