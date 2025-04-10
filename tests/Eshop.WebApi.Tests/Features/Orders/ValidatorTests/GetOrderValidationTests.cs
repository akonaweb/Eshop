using Eshop.WebApi.Features.Orders;
using FluentValidation.TestHelper;

namespace Eshop.WebApi.Tests.Features.Orders.ValidatorTests
{
    public class GetOrderValidationTests
    {
        private GetOrder.Validator validator;

        [SetUp]
        public void Setup()
        {
            validator = new GetOrder.Validator();
        }

        [Test]
        public void GetOrderValidator_IdMustBeGreaterThanZero()
        {
            var command = new GetOrder.Query(0);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Test]
        public void GetOrderValidator_IsValid()
        {
            var command = new GetOrder.Query(1);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
