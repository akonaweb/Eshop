using FluentValidation.TestHelper;
using Eshop.WebApi.Features.Categories;

namespace Eshop.WebApi.Tests.Features.Categories.ValidatorTests
{
    public class UpdateCategoryValidatorTests
    {
        private UpdateCategory.Validator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new UpdateCategory.Validator();
        }

        [Test]
        public void UpdateCategoryValidator_IdMustBeGreatherThanZero()
        {
            var command = new UpdateCategory.Command(0, new UpdateCategoryRequestDto { Name = "Category 1" });
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [TestCase(null)]
        [TestCase(" ")]
        public void UpdateCategoryValidator_NameNotNullOrEmpty(string? name)
        {
           var command = new UpdateCategory.Command(1, new UpdateCategoryRequestDto { Name = name! });
           var result = validator.TestValidate(command);
           result.ShouldHaveValidationErrorFor(x => x.Request.Name);
        }

        [Test]
        public void UpdateProductValidator_IsValid()
        {
            var requestDto = new UpdateCategoryRequestDto { Name = "Category" };
            var command = new UpdateCategory.Command(1, requestDto);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
