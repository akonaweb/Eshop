using Eshop.WebApi.Features.Products;
using FluentValidation.TestHelper;

namespace Eshop.WebApi.Tests.Features.Products.ValidatorTests
{
    public class DeleteProductValidatorTests
    {
        private DeleteProduct.Validator validator;

        [SetUp]
        public void Setup()
        {
            validator = new DeleteProduct.Validator();
        }

        [Test]
        public void DeleteProductValidator_IdMustBeGreatherThanZero()
        {
            var command = new DeleteProduct.Command(0);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Test]
        public void DeleteProductValidator_IsValid()
        {
            var command = new DeleteProduct.Command(1);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
