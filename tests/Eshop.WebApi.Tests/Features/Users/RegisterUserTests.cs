using Eshop.Persistence;
using Eshop.WebApi.Features.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Moq;
using Snapper;

namespace Eshop.WebApi.Tests.Features.Users
{
    public class RegisterUserTests : TestBase
    {
        [Test]
        public async Task RegisterUser_WithValidEmailAndPassword_ReturnsCorrectDto()
        {
            // arrange
            var email = "test@test.com";
            var password = "Test!123";

            userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), password)).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(um => um.CreateAsync(It.Is<ApplicationUser>(u => u.Email == email), password))
                .Callback<ApplicationUser, string>((user, _) => user.Id = "fake-user-id")
                .ReturnsAsync(IdentityResult.Success);

            var command = new RegisterUser.Command(new RegisterRequest { Email = email, Password = password });
            var handler = new RegisterUser.Handler(userManagerMock.Object);

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.ShouldMatchSnapshot();
        }

        [Test]
        public async Task RegisterUser_WithInvalidEmailAndPassword_ThrowsBadRequestWithError()
        {
            // arrange
            var email = "invalid email";
            var password = "invalid password";

            var identityError = new IdentityError { Code = "400", Description = "Invalid email or password." };
            var failedResult = IdentityResult.Failed(identityError);
            userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), password)).ReturnsAsync(failedResult);

            var command = new RegisterUser.Command(new RegisterRequest { Email = email, Password = password });
            var handler = new RegisterUser.Handler(userManagerMock.Object);

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.ShouldMatchSnapshot();
        }
    }
}
