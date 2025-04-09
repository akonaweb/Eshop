using Eshop.Domain;

namespace Eshop.Shared.Tests.Mocks
{
    public static class ProductMocks
    {
        public static Product GetProduct1() => new(1, "Title 1", "Description 1", 1, CategoryMocks.GetCategory1());
        public static Product GetProduct2() => new(2, "Title 2", "Description 2", 2, CategoryMocks.GetCategory2());
    }
}
