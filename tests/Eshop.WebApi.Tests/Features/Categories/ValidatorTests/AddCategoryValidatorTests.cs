using Eshop.WebApi.Features.Categories;
using Eshop.WebApi.Features.Products;
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

        [Test]
        public void AddCategory_Validator_Name_Is_Not_Empty()
        {
            var requestDto = new AddCategoryRequestDto { Name = " " };
            var command = new AddCategory.Command(requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Name);
        }
    }
}
