﻿using Eshop.WebApi.Features.Orders;
using FluentValidation.TestHelper;

namespace Eshop.WebApi.Tests.Features.Orders.ValidatorTests
{
    public class AddOrderValidatorTests
    {
        private AddOrder.Validator validator;

        [SetUp]
        public void Setup()
        {
            validator = new AddOrder.Validator();
        }

        [TestCase(null)]
        [TestCase(" ")]
        public void AddOrderValidator_CustomerNotNullOrEmpty(string? customer)
        {
            var requestDto = new AddOrderRequestDto
            {
                Customer = customer!,
                Address = "Address",
                Items = []
            };
            var command = new AddOrder.Command(requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Customer);
        }

        [TestCase(null)]
        [TestCase(" ")]
        public void AddOrderValidator_AddressNotNullOrEmpty(string? address)
        {
            var requestDto = new AddOrderRequestDto
            {
                Customer = "Customer",
                Address = address!,
                Items = []
            };
            var command = new AddOrder.Command(requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Address);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void AddOrderValidator_ProductIdMustBeGreaterThanZero(int productId)
        {
            var items = new List<AddOrderRequestDto.AddOrderItemRequestDto>
            {
                new() { ProductId = productId, Quantity = 1 }
            };

            var requestDto = new AddOrderRequestDto
            {
                Customer = "Customer",
                Address = "Address",
                Items = items
            };

            var command = new AddOrder.Command(requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Request.Items[0].ProductId");
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void AddOrderValidator_QuantityMustBeGreaterThanZero(int quantity)
        {
            var items = new List<AddOrderRequestDto.AddOrderItemRequestDto>
            {
                new() { ProductId = 1, Quantity = quantity }
            };

            var requestDto = new AddOrderRequestDto
            {
                Customer = "Customer",
                Address = "Address",
                Items = items
            };

            var command = new AddOrder.Command(requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("Request.Items[0].Quantity");
        }

        [Test]
        public void AddOrderValidator_ItemsMustHaveUniqueProductIds()
        {
            var items = new List<AddOrderRequestDto.AddOrderItemRequestDto>
            {
                new() { ProductId = 1, Quantity = 1 },
                new() { ProductId = 1, Quantity = 1 }
            };

            var requestDto = new AddOrderRequestDto
            {
                Customer = "Customer",
                Address = "Address",
                Items = items
            };

            var command = new AddOrder.Command(requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Items)
                  .WithErrorMessage("Items must have unique ProductId.");
        }


        [Test]
        public void AddOrderValidator_ItemsNotEmpty()
        {
            var requestDto = new AddOrderRequestDto
            {
                Customer = "Customer",
                Address = "Address",
                Items = []
            };
            var command = new AddOrder.Command(requestDto);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Items);
        }

        [Test]
        public void AddOrderValidator_IsValid()
        {
            var requestDto = new AddOrderRequestDto
            {
                Customer = "Customer",
                Address = "Address",
                Items =
                    [
                        new AddOrderRequestDto.AddOrderItemRequestDto
                        {
                            ProductId = 1,
                            Quantity = 1
                        },
                        new AddOrderRequestDto.AddOrderItemRequestDto
                        {
                            ProductId = 2,
                            Quantity = 2
                        },
                    ]
            };
            var command = new AddOrder.Command(requestDto);
            var result = validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}