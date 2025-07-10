using Eshop.Infrastructure;
using Eshop.Persistence;
using Eshop.Shared.Tests.Mocks;
using Eshop.WebApi.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        protected Mock<UserManager<ApplicationUser>> userManagerMock;
        protected Mock<SignInManager<ApplicationUser>> signInManagerMock;
        protected Mock<ITokenManager> tokenManagerMock;

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

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            signInManagerMock = new Mock<SignInManager<ApplicationUser>>(userManagerMock.Object, httpContextAccessorMock.Object, userClaimsPrincipalFactoryMock.Object, null!, null!, null!, null!);
            tokenManagerMock = new Mock<ITokenManager>();
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