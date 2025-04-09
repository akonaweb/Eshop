using Eshop.Domain;

namespace Eshop.Shared.Tests.Mocks
{
    public static class CategoryMocks
    {
        public static Category GetCategory1() => new(1, "Name 1");
        public static Category GetCategory2() => new(2, "Name 2");
    }
}
