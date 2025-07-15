using Eshop.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace Eshop.WebApi.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
