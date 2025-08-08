using Eshop.Persistence;
using Eshop.Shared.Tests.Mocks;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Users;
using Microsoft.AspNetCore.Identity;
using Moq;
using Snapper;
using static Eshop.WebApi.Infrastructure.TokenManager;

namespace Eshop.WebApi.Tests.Features.Users
{
    public class LoginUserTests : TestBase
    {
        [Test]
        public async Task LoginUser_WithCorrectCredentials_ReturnTokens()
        {
            // arrange
            var email = "test@test.com";
            var password = "Test!123";
            var user = new ApplicationUser(email);

            userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync(user);
            signInManagerMock.Setup(sm => sm.PasswordSignInAsync(user, password, false, false)).ReturnsAsync(SignInResult.Success);
            tokenManagerMock.Setup(x => x.GetTokens(user)).ReturnsAsync(new TokensResponse("access-token", DateTimeMocks.Now.AddMinutes(30), "refresh-token", DateTimeMocks.Now.AddDays(7)));

            var request = new LoginRequestDto { Email = email, Password = password };
            var command = new LoginUser.Command(request);
            var handler = new LoginUser.Handler(userManagerMock.Object, signInManagerMock.Object, tokenManagerMock.Object);

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.ShouldMatchSnapshot();
        }

        [Test]
        public void LoginUser_WithInvalidEmail_ThrowsUnauthorizedException()
        {
            // arrange
            var email = "test@test.com";
            var password = "Test!123";
            ApplicationUser user = null!;

            userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync(user);

            var request = new LoginRequestDto { Email = email, Password = password };
            var command = new LoginUser.Command(request);
            var handler = new LoginUser.Handler(userManagerMock.Object, signInManagerMock.Object, tokenManagerMock.Object);

            // act/assert
            Assert.ThrowsAsync<UnauthorizedException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void LoginUser_WithInvalidPassword_ThrowsUnauthorizedException()
        {
            // arrange
            var email = "test@test.com";
            var password = "Test!123";
            var user = new ApplicationUser(email);

            userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync(user);
            signInManagerMock.Setup(sm => sm.PasswordSignInAsync(user, password, false, false)).ReturnsAsync(SignInResult.Failed);

            var request = new LoginRequestDto { Email = email, Password = password };
            var command = new LoginUser.Command(request);
            var handler = new LoginUser.Handler(userManagerMock.Object, signInManagerMock.Object, tokenManagerMock.Object);

            // act/assert
            Assert.ThrowsAsync<UnauthorizedException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
