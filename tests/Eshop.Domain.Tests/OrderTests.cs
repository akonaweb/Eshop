﻿using Eshop.Shared.Tests.Mocks;
using Eshop.Shared.Tests.Utils;

namespace Eshop.Domain.Tests
{
    public class OrderTests
    {
        [Test]
        public void Order_WithValidParams_SetCorrectlyProperties()
        {
            // arrange
            var id = 0; // Zero is valid for a new object (EF)
            var customer = StringUtils.GenerateRandomString(50);
            var address = StringUtils.GenerateRandomString(500);

            var sut = new Order(id, customer, address, DateTimeMocks.Now, [OrderItemMocks.GetOrderItem1(), OrderItemMocks.GetOrderItem2()]);

            Assert.Multiple(() =>
            {
                // assert
                Assert.That(sut.Id, Is.EqualTo(id));
                Assert.That(sut.Customer, Is.EqualTo(customer));
                Assert.That(sut.Address, Is.EqualTo(address));
                Assert.That(sut.TotalPrice, Is.EqualTo(13.68m));
                Assert.That(sut.CreatedAt, Is.EqualTo(DateTimeMocks.Now));
                Assert.That(sut.Items, Has.Count.EqualTo(2));
            });
        }

        [Test]
        public void Order_WithInvalidIdParam_ThrowsException()
        {
            // act/assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Order(-1, "Customer", "Address", DateTime.UtcNow, []));
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
            Assert.Throws<ArgumentNullException>(() => new Order(0, customer!, address!, DateTime.UtcNow, []));
        }

        [Test]
        public void Order_WithDupliciteProductIdsInOrderItems_ThrowsException()
        {
            // act/assert
            var orderItems = new List<OrderItem> { OrderItemMocks.GetOrderItem1(), OrderItemMocks.GetOrderItem1() };
            Assert.Throws<InvalidOperationException>(() => new Order(0, "Customer", "Address", DateTime.UtcNow, orderItems));
        }

        [Test]
        public void OrderUpdate_WithValidParams_SetCorrectlyProperties()
        {
            // arrange
            var order = OrderMocks.GetOrder1();
            var customer = StringUtils.GenerateRandomString(50);
            var address = StringUtils.GenerateRandomString(500);

            order.Update(customer, address, [OrderItemMocks.GetOrderItem1(), OrderItemMocks.GetOrderItem2(), OrderItemMocks.GetOrderItem3()]);

            Assert.Multiple(() =>
            {
                // assert
                Assert.That(order.Customer, Is.EqualTo(customer));
                Assert.That(order.Address, Is.EqualTo(address));
                Assert.That(order.Items, Has.Count.EqualTo(3));
            });
        }

        // Customer property
        [TestCase(null, "Address")]
        [TestCase(" ", "Address")]
        [TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy", "Address")] // 51 chars
        // Address property
        [TestCase("Customer", null)]
        [TestCase("Customer", " ")]
        [TestCase("Customer", "ZbrEmS4KqO8JzyA13tdYtvb5b3B7WFiIej0PdgMlnQyC2tnS5OfVsf0boGoPpJ8YmdnEhDVBoBuE1fjKDAHwhDZnBdxQNmRzUH0SRk44sBcu4yMO3pdu63UBdXiWi02Q7nFDaNRpx3Nyi9YGeRO2L48wR3dTbhuJDVz4KwhnV89K3WW8QXpL2dFdZz0CDRTWsxNWq48x6hsgQiX4IOvMob6qVIOLfHIrUT8kSCNevxqzTRynAKrwoxkCLcEgYq7u699PupVYTbP8es3X8YvtehAP8IYA89T1AfScBA0yrhgke4OKPR9HdG6crui42yN2aINCqhPqpMPKdyAVgU7XdQFD2UYSIvDAdBWS6XJk0ftiAkrVRWZC8nK5InU49ALqAOtwVxFeo3Eevszr9sMKcYtnYQtwc6RDHnhjCz8VWViyfludPkwFQ2yJqF6jHucTl9PNZXrhG03iwzzan22clBSqjcLd2l90fcHK10ZbxyYlwm2pHv8xi")] // 501 chars
        public void OrderUpdate_WithInvalidParams_ThrowsException(string? customer, string? address)
        {
            // act/assert
            var order = OrderMocks.GetOrder1();
            Assert.Throws<ArgumentNullException>(() => order.Update(customer!, address!, [OrderItemMocks.GetOrderItem1()]));
        }

        [Test]
        public void OrderUpdate_WithDupliciteProductIdsInOrderItems_ThrowsException()
        {
            // act/assert
            var order = OrderMocks.GetOrder1();
            var orderItems = new List<OrderItem> { OrderItemMocks.GetOrderItem1(), OrderItemMocks.GetOrderItem1() };
            Assert.Throws<InvalidOperationException>(() => order.Update("Customer", "Address", orderItems));
        }
    }
}
