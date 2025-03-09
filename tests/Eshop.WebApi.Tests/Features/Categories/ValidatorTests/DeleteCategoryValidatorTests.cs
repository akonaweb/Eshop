using Eshop.WebApi.Features.Categories;
using FluentValidation.TestHelper;

namespace Eshop.WebApi.Tests.Features.Categories.ValidatorTests
{
    public class DeleteCategoryValidatorTests
    {
        private DeleteCategory.Validator? validator;

        [SetUp]
        public void Setup()
        {
            validator = new DeleteCategory.Validator();
        }

        [Test]
        public void DeleteCategoryValidator_IdMustBeGreatherThanZero()
        {
            var command = new DeleteCategory.Command(0);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Test]
        public void DeleteCategoryValidator_IsValid()
        {
            var command = new DeleteCategory.Command(1);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
