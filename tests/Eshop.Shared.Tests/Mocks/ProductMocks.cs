using Eshop.Domain;

namespace Eshop.Shared.Tests.Mocks
{
    public static class ProductMocks
    {
        public static Product GetProduct1() => new Product(1, "Title 1", "Description 1", 10, null);
        public static Product GetProduct2() => new Product(2, "Title 2", "Description 2", 20, null);
    }
}
