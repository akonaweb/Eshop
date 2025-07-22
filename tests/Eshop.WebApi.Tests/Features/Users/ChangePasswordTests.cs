using Eshop.Persistence;
using Eshop.WebApi.Features.Users;
using Microsoft.AspNetCore.Identity;
using Moq;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Users
{
    public class ChangePasswordTests : TestBase
    {
        [Test]
        public async Task ChangePasswordUser_WithValidPasswords_ReturnsPasswordChangedSuccessfully()
        {
            // arrange
            var userId = Guid.NewGuid();
            var currentPassword = "CurrentPassword!123";
            var newPassword = "NewPassword!123";
            var user = new ApplicationUser("test@test.com");

            userContextMock.Setup(x => x.GetUserId()).Returns(userId);
            userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            userManagerMock.Setup(x => x.ChangePasswordAsync(user, currentPassword, newPassword))
                           .ReturnsAsync(IdentityResult.Success);

            var request = new ChangePasswordRequest
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };

            var handler = new ChangePassword.Handler(userManagerMock.Object, userContextMock.Object);
            var command = new ChangePassword.Command(request);

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.ShouldMatchSnapshot();
        }

        [Test]
        public async Task ChangePasswordUser_WhenUserNotFound_ReturnsNotFound()
        {
            // arrange
            var userId = Guid.NewGuid();
            var currentPassword = "CurrentPassword!123";
            var newPassword = "NewPassword!123";
            ApplicationUser user = null!;

            userContextMock.Setup(x => x.GetUserId()).Returns(userId);
            userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString())).ReturnsAsync(user);

            var request = new ChangePasswordRequest
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };

            var handler = new ChangePassword.Handler(userManagerMock.Object, userContextMock.Object);
            var command = new ChangePassword.Command(request);

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.ShouldMatchSnapshot();
        }

        [Test]
        public async Task ChangePasswordUser_WithInvalidPassword_ReturnsBadRequest()
        {
            // arrange
            var userId = Guid.NewGuid();
            var currentPassword = "CurrentPassword!123";
            var newPassword = "BadPassword";
            var user = new ApplicationUser("test@test.com");

            userContextMock.Setup(x => x.GetUserId()).Returns(userId);
            userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            userManagerMock.Setup(x => x.ChangePasswordAsync(user, currentPassword, newPassword))
                           .ReturnsAsync(IdentityResult.Failed([new IdentityError { Code = "400", Description = "Incorrect new password." }]));

            var request = new ChangePasswordRequest
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };

            var handler = new ChangePassword.Handler(userManagerMock.Object, userContextMock.Object);
            var command = new ChangePassword.Command(request);

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.ShouldMatchSnapshot();
        }
    }
}
