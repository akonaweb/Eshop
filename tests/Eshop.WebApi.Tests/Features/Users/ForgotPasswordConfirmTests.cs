using Eshop.Persistence;
using Eshop.WebApi.Features.Users;
using Microsoft.AspNetCore.Identity;
using Moq;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Users
{
    public class ForgotPasswordConfirmTests : TestBase
    {
        [Test]
        public async Task ForgotPasswordConfirm_WithValidReset_ReturnsPasswordChangedSuccessfully()
        {
            // arrange
            var email = "test@test.com";
            var user = new ApplicationUser(email);
            var resetCode = "valid-reset-code";
            var newPassword = "Test!123";

            userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync(user);
            userManagerMock.Setup(um => um.ResetPasswordAsync(user, resetCode, newPassword))
                .ReturnsAsync(IdentityResult.Success);

            var request = new ForgotPasswordConfirmRequestDto { Email = email, ResetCode = resetCode, NewPassword = newPassword };
            var handler = new ForgotPasswordConfirm.Handler(userManagerMock.Object);
            var command = new ForgotPasswordConfirm.Command(request);

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.ShouldMatchSnapshot();
        }

        [Test]
        public async Task ForgotPasswordConfirm_UserNotFound_ReturnsBadRequest()
        {
            // arrange
            var email = "test@test.com";
            ApplicationUser user = null!;
            var resetCode = "some-code";
            var newPassword = "Test!123";

            userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync(user);

            var request = new ForgotPasswordConfirmRequestDto { Email = email, ResetCode = resetCode, NewPassword = newPassword };
            var command = new ForgotPasswordConfirm.Command(request);
            var handler = new ForgotPasswordConfirm.Handler(userManagerMock.Object);

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.ShouldMatchSnapshot();
        }

        [Test]
        public async Task ForgotPasswordConfirm_ResetFails_ReturnsBadRequestWithErrors()
        {
            // Arrange
            var email = "test@test.com";
            var user = new ApplicationUser(email);
            var resetCode = "invalid-reset-code";
            var newPassword = "Test!123";
            var identityErrors = new List<IdentityError>
            {
                new IdentityError { Code = "InvalidToken", Description = "The token is invalid or expired." }
            };

            userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync(user);
            userManagerMock.Setup(um => um.ResetPasswordAsync(user, resetCode, newPassword))
                .ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));

            var request = new ForgotPasswordConfirmRequestDto { Email = email, ResetCode = resetCode, NewPassword = newPassword };
            var command = new ForgotPasswordConfirm.Command(request);
            var handler = new ForgotPasswordConfirm.Handler(userManagerMock.Object);

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.ShouldMatchSnapshot();
        }
    }
}
