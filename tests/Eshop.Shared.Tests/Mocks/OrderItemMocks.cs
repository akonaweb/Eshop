using Eshop.Domain;

namespace Eshop.Shared.Tests.Mocks
{
    public static class OrderItemMocks
    {
        public static OrderItem GetOrderItem1() => new(1, 2.34m, ProductMocks.GetProduct1());
        public static OrderItem GetOrderItem2() => new(2, 5.67m, ProductMocks.GetProduct2());
        public static OrderItem GetOrderItem3() => new(3, 6.78m, ProductMocks.GetProduct3());
    }
}
