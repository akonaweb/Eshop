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
            var requestDto = new UpdateOrderRequestDto
            {
                Customer = "Customer",
                Address = "Address",
                Items = []
            };
            var command = new UpdateOrder.Command(id, requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [TestCase(null)]
        [TestCase(" ")]
        public void UpdateOrderValidator_CustomerNotNullOrEmpty(string? customer)
        {
            var requestDto = new UpdateOrderRequestDto
            {
                Customer = customer!,
                Address = "Address",
                Items = []
            };
            var command = new UpdateOrder.Command(1, requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Customer);
        }

        [TestCase(null)]
        [TestCase(" ")]
        public void UpdateOrderValidator_AddressNotNullOrEmpty(string? address)
        {
            var requestDto = new UpdateOrderRequestDto
            {
                Customer = "Customer",
                Address = address!,
                Items = []
            };
            var command = new UpdateOrder.Command(1, requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Address);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void UpdateOrderValidator_ProductIdMustBeGreaterThanZero(int productId)
        {
            var items = new List<UpdateOrderRequestDto.UpdateOrderItemRequestDto>
            {
                new() { ProductId = productId, Quantity = 1 }
            };

            var requestDto = new UpdateOrderRequestDto
            {
                Customer = "Customer",
                Address = "Address",
                Items = items
            };

            var command = new UpdateOrder.Command(1, requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Request.Items[0].ProductId");
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void UpdateOrderValidator_QuantityMustBeGreaterThanZero(int quantity)
        {
            var items = new List<UpdateOrderRequestDto.UpdateOrderItemRequestDto>
            {
                new() { ProductId = 1, Quantity = quantity }
            };

            var requestDto = new UpdateOrderRequestDto
            {
                Customer = "Customer",
                Address = "Address",
                Items = items
            };

            var command = new UpdateOrder.Command(1, requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Request.Items[0].Quantity");
        }

        [Test]
        public void UpdateOrderValidator_ItemsMustHaveUniqueProductIds()
        {
            var items = new List<UpdateOrderRequestDto.UpdateOrderItemRequestDto>
            {
                new() { ProductId = 1, Quantity = 1 },
                new() { ProductId = 1, Quantity = 1 }
            };

            var requestDto = new UpdateOrderRequestDto
            {
                Customer = "Customer",
                Address = "Address",
                Items = items
            };

            var command = new UpdateOrder.Command(1, requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Items)
                  .WithErrorMessage("Items must have unique ProductId.");
        }


        [Test]
        public void UpdateOrderValidator_ItemsNotEmpty()
        {
            var requestDto = new UpdateOrderRequestDto
            {
                Customer = "Customer",
                Address = "Address",
                Items = []
            };
            var command = new UpdateOrder.Command(1, requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Items);
        }

        [Test]
        public void UpdateOrderValidator_IsValid()
        {
            var requestDto = new UpdateOrderRequestDto
            {
                Customer = "Customer",
                Address = "Address",
                Items =
                    [
                        new UpdateOrderRequestDto.UpdateOrderItemRequestDto
                        {
                            ProductId = 1,
                            Quantity = 1
                        },
                        new UpdateOrderRequestDto.UpdateOrderItemRequestDto
                        {
                            ProductId = 2,
                            Quantity = 2
                        },
                    ]
            };
            var command = new UpdateOrder.Command(1, requestDto);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
