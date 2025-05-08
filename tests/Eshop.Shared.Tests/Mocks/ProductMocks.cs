using Eshop.Domain;

namespace Eshop.Shared.Tests.Mocks
{
    public static class ProductMocks
    {
        public static Product GetProduct1() => new(1, "Product Title 1", "Product Description 1", 1.23m, CategoryMocks.GetCategory1());
        public static Product GetProduct2() => new(2, "Product Title 2", "Product Description 2", 2.34m, CategoryMocks.GetCategory2());
        public static Product GetProduct3() => new(3, "Product Title 3", "Product Description 3", 3.45m, CategoryMocks.GetCategory3());
    }
}
