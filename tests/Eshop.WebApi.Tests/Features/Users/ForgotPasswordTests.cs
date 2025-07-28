using Eshop.Persistence;
using Eshop.WebApi.Features.Users;
using Moq;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Users
{
    public class ForgotPasswordTests : TestBase
    {
        [Test]
        public async Task ForgotPassword_WithCorrectCredentials_ReturnsPasswordResetRequest()
        {
            // arrange
            var email = "test@test.com";
            var user = new ApplicationUser(email);
            var token = "test-reset-token";

            userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync(user);
            userManagerMock.Setup(um => um.GeneratePasswordResetTokenAsync(user)).ReturnsAsync(token);

            var request = new ForgotPasswordRequestDto { Email = email };
            var command = new ForgotPassword.Command(request);
            var handler = new ForgotPassword.Handler(userManagerMock.Object);

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.ShouldMatchSnapshot();
        }

        [Test]
        public async Task ForgotPassword_WhenUserNotFound_ReturnsNotFound()
        {
            // arrange
            var email = "test@test.com";
            ApplicationUser user = null!;
            var token = "test-reset-token";

            userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync(user);
            userManagerMock.Setup(um => um.GeneratePasswordResetTokenAsync(user)).ReturnsAsync(token);

            var request = new ForgotPasswordRequestDto { Email = email };
            var command = new ForgotPassword.Command(request);
            var handler = new ForgotPassword.Handler(userManagerMock.Object);

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.ShouldMatchSnapshot();
        }
    }
}
