using Eshop.Domain;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Products;
using FluentValidation.TestHelper;

namespace Eshop.WebApi.Tests.Features.Products
{
    public class UpdateProductTests : TestBase
    {
        private UpdateProduct.Validator _validator;

        [SetUp]
        public async Task Seed()
        {
            _validator = new UpdateProduct.Validator();
            var category = await dbContext.Categories.AddAsync(new Category(0, "Category 1"));
            await dbContext.Products.AddAsync(new Product(0, "Test product", "bla", 1, category.Entity));
            await dbContext.SaveChangesAsync();
        }

        [Test]
        public void Validator_Should_Have_Error_When_Id_Is_Less_Than_Or_Equal_To_Zero()
        {
            var command = new UpdateProduct.Command(0, new UpdateProductRequestDto { Title = "Valid", Description = "Valid", Price = 10, CategoryId = 1 });
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Test]
        public void Validator_Should_Have_Error_When_Title_Is_Empty()
        {
            var command = new UpdateProduct.Command(1, new UpdateProductRequestDto { Title = "", Description = "Valid", Price = 10, CategoryId = 1 });
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Title);
        }

        [Test]
        public void Validator_Should_Have_Error_When_Description_Is_Empty()
        {
            var command = new UpdateProduct.Command(1, new UpdateProductRequestDto { Title = "Valid", Description = "", Price = 10, CategoryId = 1 });
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Description);
        }

        [Test]
        public void Validator_Should_Have_Error_When_Price_Is_Less_Than_Or_Equal_To_Zero()
        {
            var command = new UpdateProduct.Command(1, new UpdateProductRequestDto { Title = "Valid", Description = "Valid", Price = 0, CategoryId = 1 });
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Price);
        }

        [Test]
        public void Validator_Should_Have_Error_When_CategoryId_Is_Less_Than_Or_Equal_To_Zero()
        {
            var command = new UpdateProduct.Command(1, new UpdateProductRequestDto { Title = "Valid", Description = "Valid", Price = 10, CategoryId = 0 });
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.CategoryId);
        }

        [Test]
        public void Validator_Should_Not_Have_Error_When_Command_Is_Valid()
        {
            var command = new UpdateProduct.Command(1, new UpdateProductRequestDto { Title = "Valid", Description = "Valid", Price = 10, CategoryId = 1 });
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void UpdateProduct_ThrowsArgumentNotFoundException()
        {
            // act
            var requestDto = new UpdateProductRequestDto
            {
                Title = "Test product",
                Description = "bla",
                Price = 1,
                CategoryId = 1
            };
            var query = new UpdateProduct.Command(0, requestDto);
            var handler = new UpdateProduct.Handler(dbContext);

            // assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}