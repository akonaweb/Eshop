using Eshop.WebApi.Features.Products;
using FluentValidation.TestHelper;

namespace Eshop.WebApi.Tests.Features.Products.ValidatorTests
{
    public class GetProductValidatorTests
    {
        private GetProduct.Validator validator;

        [SetUp]
        public void Setup()
        {
            validator = new GetProduct.Validator();
        }

        [Test]
        public void GetProductValidator_IdMustBeGreatherThanZero()
        {
            var command = new GetProduct.Query(0);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Test]
        public void GetProductValidator_IsValid()
        {
            var command = new GetProduct.Query(1);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
