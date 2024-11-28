﻿using Eshop.Domain;
using Eshop.WebApi.Features.Products;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Products
{
    public class GetProductsTests : TestBase
    {
        [SetUp]
        public async Task Seed()
        {
            var category = await dbContext.Categories.AddAsync(new Category(0, "Category 1"));
            await dbContext.Products.AddAsync(new Product(0, "Test product", "bla", 1, category.Entity));
            await dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task GetProducts_ReturnsCorrectDto()
        {
            // arrange
            var query = new GetProducts.Query();
            var handler = new GetProducts.Handler(dbContext);

            // act
            var sut = await handler.Handle(query, CancellationToken.None);

            // assert
            sut.ShouldMatchSnapshot();
        }
    }
}
