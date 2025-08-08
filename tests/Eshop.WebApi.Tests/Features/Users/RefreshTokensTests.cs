using Eshop.Persistence;
using Eshop.Shared.Tests.Mocks;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Users;
using Moq;
using Snapper;
using static Eshop.WebApi.Infrastructure.TokenManager;

namespace Eshop.WebApi.Tests.Features.Users
{
    public class RefreshTokensTests : TestBase
    {
        [Test]
        public async Task RefreshTokens_WithValidRefreshToken_ReturnsNewTokens()
        {
            // arrange
            var refreshToken = "refresh-token";
            var email = "test@test.com";
            var user = new ApplicationUser(email);
            user.UpdateRefreshToken(refreshToken, DateTimeMocks.Now.AddDays(1)); 
            await dbContext.Users.AddAsync(user, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            userManagerMock.Setup(x => x.Users).Returns(dbContext.Users);
            userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync(user);
            tokenManagerMock.Setup(x => x.GetTokens(user)).ReturnsAsync(new TokensResponse("access-token", DateTimeMocks.Now.AddMinutes(30), "refresh-token", DateTimeMocks.Now.AddDays(7)));

            var command = new RefreshTokens.Command(refreshToken);
            var handler = new RefreshTokens.Handler(tokenManagerMock.Object, dateTimeProviderMock.Object, userManagerMock.Object);

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.ShouldMatchSnapshot();
        }

        [Test]
        public void RefreshTokens_WithNotFoundUserByRefreshToken_ThrowsUnauthorizedException()
        {
            // arrange
            var refreshToken = "refresh-token";
            var email = "test@test.com";
            var user = new ApplicationUser(email);
            
            userManagerMock.Setup(x => x.Users).Returns(dbContext.Users);
            userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync(user);
            tokenManagerMock.Setup(x => x.GetTokens(user)).ReturnsAsync(new TokensResponse("access-token", DateTimeMocks.Now.AddMinutes(30), "refresh-token", DateTimeMocks.Now.AddDays(7)));

            var command = new RefreshTokens.Command(refreshToken);
            var handler = new RefreshTokens.Handler(tokenManagerMock.Object, dateTimeProviderMock.Object, userManagerMock.Object);

            // act/assert
            Assert.ThrowsAsync<UnauthorizedException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task RefreshTokens_WithExpiredRefreshToken_ThrowsUnauthorizedException()
        {
            // arrange
            var refreshToken = "refresh-token";
            var email = "test@test.com";
            var user = new ApplicationUser(email);
            user.UpdateRefreshToken(refreshToken, DateTimeMocks.Now.AddDays(-1));
            await dbContext.Users.AddAsync(user, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            userManagerMock.Setup(x => x.Users).Returns(dbContext.Users);
            userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync(user);
            tokenManagerMock.Setup(x => x.GetTokens(user)).ReturnsAsync(new TokensResponse("access-token", DateTimeMocks.Now.AddMinutes(30), "refresh-token", DateTimeMocks.Now.AddDays(7)));

            var command = new RefreshTokens.Command(refreshToken);
            var handler = new RefreshTokens.Handler(tokenManagerMock.Object, dateTimeProviderMock.Object, userManagerMock.Object);

            // act/assert
            Assert.ThrowsAsync<UnauthorizedException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}