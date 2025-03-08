using Eshop.WebApi.Features.Products;
using FluentValidation.TestHelper;

namespace Eshop.WebApi.Tests.Features.Products.ValidatorTests
{
    public class AddProductValidatorTests
    {
        private AddProduct.Validator validator;

        [SetUp]
        public void Setup()
        {
            validator = new AddProduct.Validator();
        }

        [TestCase(null)]
        [TestCase(" ")]
        public void AddProductValidator_TitleNotNullOrEmpty(string? title)
        {
            var requestDto = new AddProductRequestDto { Title = title!, Description = "Descritption" };
            var command = new AddProduct.Command(requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Title);
        }

        [TestCase(null)]
        [TestCase(" ")]
        public void AddProductValidator_DescriptionNotNullOrEmpty(string? description)
        {
            var requestDto = new AddProductRequestDto { Title = "Title", Description = description! };
            var command = new AddProduct.Command(requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Description);
        }

        [Test]
        public void AddProductValidator_PriceMustBePositive()
        {
            var requestDto = new AddProductRequestDto { Title = "Title", Description = "Description", Price = -1 };
            var command = new AddProduct.Command(requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Price);
        }

        [Test]
        public void AddProductValidator_PriceShouldAllowZero()
        {
            var requestDto = new AddProductRequestDto { Title = "Title", Description = "Description", Price = 0 };
            var command = new AddProduct.Command(requestDto);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Request.Price);
        }

        [Test]
        public void AddProductValidator_CategoryShouldBeGreatherThanZeroIfNotNull()
        {
            var requestDto = new AddProductRequestDto { Title = "Title", Description = "Description", Price = 0, CategoryId = 0 };
            var command = new AddProduct.Command(requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.CategoryId);
        }

        [Test]
        public void AddProductValidator_CategoryShouldAllowNull()
        {
            var requestDto = new AddProductRequestDto { Title = "Title", Description = "Description", Price = 0, CategoryId = null };
            var command = new AddProduct.Command(requestDto);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Request.CategoryId);
        }

        [Test]
        public void AddProductValidator_IsValid()
        {
            var requestDto = new AddProductRequestDto { Title = "Title", Description = "Description", Price = 0, CategoryId = 1 };
            var command = new AddProduct.Command(requestDto);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
