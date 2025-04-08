﻿using Eshop.Domain.Tests.Mocks;
using Eshop.Domain.Tests.Utils;
using System.Reflection;

namespace Eshop.Domain.Tests
{
    [TestFixture]
    public class OrderTests
    {
        [Test]
        public void Order_WithValidParams_SetCorrectlyProperties()
        {
            // arrange
            var id = 0; // Zero is valid for a new object (EF)
            var customer = StringUtils.GenerateRandomString(50);
            var address = StringUtils.GenerateRandomString(500);

            var sut = new Order(id, customer, address);

            Assert.Multiple(() =>
            {
                // assert
                Assert.That(sut.Id, Is.EqualTo(id));
                Assert.That(sut.Customer, Is.EqualTo(customer));
                Assert.That(sut.Address, Is.EqualTo(address));
                Assert.That(sut.Items, Has.Count.EqualTo(0));
                // NOTE: we have no option to properly test CreatedAt
            });
        }

        [Test]
        public void Order_WithInvalidIdParam_ThrowsException()
        {
            // act/assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Order(-1, "Customer", "Address"));
        }

        // Customer property
        [TestCase(null, "Address")]
        [TestCase(" ", "Address")]
        [TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy", "Address")] // 51 chars
        // Address property
        [TestCase("Customer", null)]
        [TestCase("Customer", " ")]
        [TestCase("Customer", "ZbrEmS4KqO8JzyA13tdYtvb5b3B7WFiIej0PdgMlnQyC2tnS5OfVsf0boGoPpJ8YmdnEhDVBoBuE1fjKDAHwhDZnBdxQNmRzUH0SRk44sBcu4yMO3pdu63UBdXiWi02Q7nFDaNRpx3Nyi9YGeRO2L48wR3dTbhuJDVz4KwhnV89K3WW8QXpL2dFdZz0CDRTWsxNWq48x6hsgQiX4IOvMob6qVIOLfHIrUT8kSCNevxqzTRynAKrwoxkCLcEgYq7u699PupVYTbP8es3X8YvtehAP8IYA89T1AfScBA0yrhgke4OKPR9HdG6crui42yN2aINCqhPqpMPKdyAVgU7XdQFD2UYSIvDAdBWS6XJk0ftiAkrVRWZC8nK5InU49ALqAOtwVxFeo3Eevszr9sMKcYtnYQtwc6RDHnhjCz8VWViyfludPkwFQ2yJqF6jHucTl9PNZXrhG03iwzzan22clBSqjcLd2l90fcHK10ZbxyYlwm2pHv8xi")] // 501 chars
        public void Order_WithInvalidParams_ThrowsException(string? customer, string? address)
        {
            // act/assert
            Assert.Throws<ArgumentNullException>(() => new Order(0, customer!, address!));
        }

        [Test]
        public void Order_AddItem_SetCorrectlyOrderItem()
        {
            // arrange
            var sut = OrderMocks.GetOrder1();

            // act
            sut.AddItem(OrderItemMocks.GetOrderItem1());
            sut.AddItem(OrderItemMocks.GetOrderItem2());

            // assert
            Assert.That(sut.Items, Has.Count.EqualTo(2));
        }

        [Test]
        public void Order_WithValidParams_SetsCreatedAtWithinReasonableRange()
        {
            // arrange
            var id = 0;
            var customer = StringUtils.GenerateRandomString(50);
            var address = StringUtils.GenerateRandomString(500);

            var sut = new Order(id, customer, address);

            // act
            var createdAt = sut.CreatedAt;

            // assert
            Assert.That(createdAt, Is.InRange(DateTime.UtcNow.AddSeconds(-5), DateTime.UtcNow.AddSeconds(5)));
        }

        [Test]
        public void Order_AddItem_UpdatesItemsCollectionCorrectly()
        {
            // arrange
            var sut = OrderMocks.GetOrder1();
            var newItem = OrderItemMocks.GetOrderItem1();

            // act
            sut.AddItem(newItem);

            // assert
            Assert.That(sut.Items, Has.Count.EqualTo(1));
            Assert.That(sut.Items.First(), Is.EqualTo(newItem));
        }

        [Test]
        public void Order_AddItem_AddsMultipleDistinctItemsCorrectly()
        {
            // arrange
            var sut = OrderMocks.GetOrder1();
            var item1 = OrderItemMocks.GetOrderItem1();
            var item2 = OrderItemMocks.GetOrderItem2();

            // act
            sut.AddItem(item1);
            sut.AddItem(item2);

            // assert
            Assert.That(sut.Items, Has.Count.EqualTo(2));
            Assert.That(sut.Items.First(), Is.EqualTo(item1));
            Assert.That(sut.Items.Last(), Is.EqualTo(item2));
        }

        [Test]
        public void Order_AddItem_AddsFirstItemCorrectly()
        {
            // arrange
            var sut = OrderMocks.GetOrder1();
            var item = OrderItemMocks.GetOrderItem1();

            // act
            sut.AddItem(item);

            // assert
            Assert.That(sut.Items, Has.Count.EqualTo(1));
            Assert.That(sut.Items.First(), Is.EqualTo(item));
        }

        [Test]
        public void Order_AddItem_ThrowsExceptionWhenAddingExistingProductTwice()
        {
            // arrange
            var sut = OrderMocks.GetOrder1();

            // act/assert
            sut.AddItem(OrderItemMocks.GetOrderItem1());
            var exception = Assert.Throws<InvalidOperationException>(() => sut.AddItem(OrderItemMocks.GetOrderItem1()));
            StringAssert.Contains("already exists", exception.Message);
        }

        [Test]
        public void Order_PrivateConstructor_InstantiatesSuccessfully()
        {
            // act
            var constructor = typeof(Order).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);

            var sut = constructor?.Invoke(null);

            // assert
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void Order_WithMaxCustomerNameLength_DoesNotThrowException()
        {
            // arrange
            var customer = new string('A', 50);
            var address = "Valid Address";

            // act
            var sut = new Order(1, customer, address);

            // assert
            Assert.That(sut.Customer, Is.EqualTo(customer));
        }

        [Test]
        public void Order_WithMaxAddressLength_DoesNotThrowException()
        {
            // arrange
            var customer = "Valid Customer";
            var address = new string('A', 500);

            // act
            var sut = new Order(1, customer, address);

            // assert
            Assert.That(sut.Address, Is.EqualTo(address));
        }

        [Test]
        public void Order_CalculateTotalPrice_WithValidItems_ReturnsCorrectTotal()
        {
            // arrange
            var order = OrderMocks.GetOrder1();
            var item1 = OrderItemMocks.GetOrderItem1();
            var item2 = OrderItemMocks.GetOrderItem2();

            order.AddItem(item1);
            order.AddItem(item2);

            // act
            var totalPrice = order.TotalPrice;

            // assert
            var expectedTotal = item1.TotalPrice + item2.TotalPrice;
            Assert.That(totalPrice, Is.EqualTo(expectedTotal));
        }
    }
}
