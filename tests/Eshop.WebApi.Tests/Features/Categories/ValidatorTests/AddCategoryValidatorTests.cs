using Eshop.WebApi.Features.Categories;
using FluentValidation.TestHelper;

namespace Eshop.WebApi.Tests.Features.Categories.ValidatorTests
{
    public class AddCategoryValidatorTests
    {
        private AddCategory.Validator? validator;

        [SetUp]
        public void Setup()
        {
            validator = new AddCategory.Validator();
        }

        [TestCase(null)]
        [TestCase(" ")]
        public void AddCategoryValidator_NameNotNullOrEmpty(string? name)
        {
            var requestDto = new AddCategoryRequestDto { Name = name! };
            var command = new AddCategory.Command(requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Name);
        }

        [Test]
        public void AddCategoryValidator_IsValid()
        {
            var requestDto = new AddCategoryRequestDto { Name = "Category 1" };
            var command = new AddCategory.Command(requestDto);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
