using Eshop.Infrastructure;
using Eshop.Persistence;
using Eshop.Shared.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Eshop.WebApi.Tests
{
    public abstract class TestBase
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        protected EshopDbContext dbContext;
        protected DbContextOptions<EshopDbContext> dbContextOptions;
        protected static DateTime Now => DateTimeMocks.Now;
        protected Mock<IDateTimeProvider> dateTimeProviderMock;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        [SetUp]
        public async Task SetUp()
        {
            dbContextOptions = new DbContextOptionsBuilder<EshopDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            dbContext = new EshopDbContext(dbContextOptions);

            await dbContext.Database.OpenConnectionAsync();
            await dbContext.Database.EnsureCreatedAsync();

            dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(x => x.Now).Returns(Now);
        }

        [TearDown]
        public async Task TearDown()
        {
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.CloseConnectionAsync();
            await dbContext.DisposeAsync();
        }
    }
}