using Eshop.WebApi.Features.Categories;
using Eshop.WebApi.Features.Products;
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
        public void DeleteCategories_Validator_Id_Is_Positive()
        {
            var command = new DeleteCategory.Command(0);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Test]
        public void DeleteProduct_Validator_Command_Is_Valid()
        {
            var command = new DeleteCategory.Command(1);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
