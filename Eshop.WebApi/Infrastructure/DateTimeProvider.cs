using Eshop.Infrastructure;

namespace Eshop.WebApi.Infrastructure
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
