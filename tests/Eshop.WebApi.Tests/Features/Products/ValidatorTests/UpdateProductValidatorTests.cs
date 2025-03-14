using Eshop.WebApi.Features.Products;
using FluentValidation.TestHelper;

namespace Eshop.WebApi.Tests.Features.Products.ValidatorTests
{
    public class UpdateProductValidatorTests
    {
        private UpdateProduct.Validator validator;

        [SetUp]
        public void Setup()
        {
            validator = new UpdateProduct.Validator();
        }

        [Test]
        public void UpdateProductValidator_IdMustBeGreatherThanZero()
        {
            var command = new UpdateProduct.Command(0, new UpdateProductRequestDto { Title = "Title", Description = "Description" });
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [TestCase(null)]
        [TestCase(" ")]
        public void UpdateProductValidator_TitleNotNullOrEmpty(string? title)
        {
            var command = new UpdateProduct.Command(0, new UpdateProductRequestDto { Title = title!, Description = "Description" });
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [TestCase(null)]
        [TestCase(" ")]
        public void UpdateProductValidator_DescriptionNotNullOrEmpty(string? description)
        {
            var command = new UpdateProduct.Command(0, new UpdateProductRequestDto { Title = "Title", Description = description! });
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Test]
        public void UpdateProductValidator_PriceShouldNotBeNegative()
        {
            var requestDto = new UpdateProductRequestDto { Title = "Title", Description = "Description", Price = -1 };
            var command = new UpdateProduct.Command(1, requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Price);
        }

        [Test]
        public void UpdateProductValidator_PriceShouldAllowZero()
        {
            var requestDto = new UpdateProductRequestDto { Title = "Title", Description = "Description", Price = 0 };
            var command = new UpdateProduct.Command(1, requestDto);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Request.Price);
        }

        [Test]
        public void UpdateProductValidator_CategoryShouldBeGreatherThanZeroIfNotNull()
        {
            var requestDto = new UpdateProductRequestDto { Title = "Title", Description = "Description", Price = 0, CategoryId = 0 };
            var command = new UpdateProduct.Command(1, requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.CategoryId);
        }

        [Test]
        public void UpdateProductValidator_CategoryShouldAllowNull()
        {
            var requestDto = new UpdateProductRequestDto { Title = "Title", Description = "Description", Price = 0, CategoryId = null };
            var command = new UpdateProduct.Command(1, requestDto);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Request.CategoryId);
        }

        [Test]
        public void UpdateProductValidator_IsValid()
        {
            var requestDto = new UpdateProductRequestDto { Title = "Title", Description = "Description", Price = 0, CategoryId = 1 };
            var command = new UpdateProduct.Command(1, requestDto);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
