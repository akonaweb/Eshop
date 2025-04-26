using Eshop.WebApi.Features.Orders;
using FluentValidation.TestHelper;

namespace Eshop.WebApi.Tests.Features.Orders.ValidatorTests
{
    public class DeleteOrderValidatorTests
    {
        private DeleteOrder.Validator? validator;

        [SetUp]
        public void Setup()
        {
            validator = new DeleteOrder.Validator();
        }

        [Test]
        public void DeleteOrderValidator_IdMustBeGreatherThanZero()
        {
            var command = new DeleteOrder.Command(0);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Test]
        public void DeleteOrderValidator_IsValid()
        {
            var command = new DeleteOrder.Command(1);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
