using Eshop.Persistence;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Features.Products;
using Eshop.WebApi.Features.Users;
using Eshop.WebApi.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
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
            tokenManagerMock.Setup(x => x.GetTokens(user)).ReturnsAsync(new TokensResponse("access-token", "refresh-token"));

            var handler = new LoginUser.Handler(userManagerMock.Object, signInManagerMock.Object, tokenManagerMock.Object);

            var request = new LoginRequest
            {
                Email = email,
                Password = password
            };

            // act
            var command = new LoginUser.Command(request);
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

            var request = new LoginRequest
            {
                Email = email,
                Password = password
            };
            var command = new LoginUser.Command(request);
            var handler = new LoginUser.Handler(userManagerMock.Object, signInManagerMock.Object, tokenManagerMock.Object);

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

            var request = new LoginRequest
            {
                Email = email,
                Password = password
            };
            var command = new LoginUser.Command(request);
            var handler = new LoginUser.Handler(userManagerMock.Object, signInManagerMock.Object, tokenManagerMock.Object);

            Assert.ThrowsAsync<UnauthorizedException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
