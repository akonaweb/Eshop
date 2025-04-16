using Eshop.WebApi.Features.Orders;
using FluentValidation.TestHelper;

namespace Eshop.WebApi.Tests.Features.Orders.ValidatorTests
{
    public class GetCartValidatorTests
    {
        private GetCart.Validator validator;

        [SetUp]
        public void Setup()
        {
            validator = new GetCart.Validator();
        }

        [Test]
        public void GetCartValidator_ProductIdMustBeGreaterThanZero()
        {
            var command = new GetCart.Query(new List<GetCartRequestDto> { new GetCartRequestDto { ProductId = 0, Quantity = 1 } });
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Request[0].ProductId");
        }


        [Test]
        public void GetCartValidator_QuantityMustBeGreaterThanZero()
        {
            var command = new GetCart.Query(new List<GetCartRequestDto> { new GetCartRequestDto { ProductId = 1, Quantity = 0 } });
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Request[0].Quantity");
        }

        [Test]
        public void GetCartValidator_ItemsMustHaveUniqueProductId()
        {
            var command = new GetCart.Query(new List<GetCartRequestDto>
                {
                    new GetCartRequestDto { ProductId = 1, Quantity = 1 },
                    new GetCartRequestDto { ProductId = 1, Quantity = 2 }
                });
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request);
        }

        [Test]
        public void GetCartValidator_IsValid()
        {
            var command = new GetCart.Query(new List<GetCartRequestDto> { new GetCartRequestDto { ProductId = 1, Quantity = 1 } });
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
