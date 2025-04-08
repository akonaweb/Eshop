namespace Eshop.Domain.Tests.Mocks
{
    public static class OrderMocks
    {
        public static Order GetOrder1() => new(1, "Customer 1", "Address 1");
    }
}
