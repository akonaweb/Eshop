using Eshop.WebApi.Features.Orders;
using FluentValidation.TestHelper;

namespace Eshop.WebApi.Tests.Features.Orders.ValidatorTests
{
    public class UpdateOrderValidatorTests
    {
        private UpdateOrder.Validator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new UpdateOrder.Validator();
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void UpdateOrderValidator_IdMustBeGreaterThanZero(int id)
        {
            var command = new UpdateOrder.Command(id, "Customer", "Address");
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void UpdateOrderValidator_CustomerMustNotBeEmpty(string? customer)
        {
            var command = new UpdateOrder.Command(1, customer!, "Address");
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Customer);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void UpdateOrderValidator_AddressMustNotBeEmpty(string? address)
        {
            var command = new UpdateOrder.Command(1, "Customer", address!);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Address);
        }

        [Test]
        public void UpdateOrderValidator_IsValid()
        {
            var command = new UpdateOrder.Command(1, "Valid Customer", "Valid Address");
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
