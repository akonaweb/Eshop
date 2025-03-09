using Eshop.WebApi.Features.Categories;
using FluentValidation.TestHelper;

namespace Eshop.WebApi.Tests.Features.Categories.ValidatorTests
{
    public class GetCategoryValidatorTests
    {
        private GetCategory.Validator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new GetCategory.Validator();
        }

        [Test]
        public void GetCategoryValidator_IdMustBeGreatherThanZero()
        {
            var command = new GetCategory.Query(0);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Test]
        public void GetCategorytValidator_IsValid()
        {
            var command = new GetCategory.Query(1);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
