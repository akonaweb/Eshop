using Eshop.Persistence;
using Eshop.WebApi.Features.Users;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using Snapper;
using System.Threading;
using System.Threading.Tasks;

namespace Eshop.WebApi.Tests.Features.Users
{
    public class RegisterUserTests
    {
        [Test]
        public async Task RegisterUser_ReturnsCorrectDto()
        {
            // Arrange
            var email = "test@test.com";
            var password = "Test!123";

            var request = new RegisterRequest
            {
                Email = email,
                Password = password
            };

            var command = new RegisterUser.Command(request);

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null
            );

            userManagerMock
                .Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), password))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock
                .Setup(um => um.CreateAsync(It.Is<ApplicationUser>(u => u.Email == email), password))
                .Callback<ApplicationUser, string>((user, _) => user.Id = "fake-user-id")
                .ReturnsAsync(IdentityResult.Success);

            var handler = new RegisterUser.Handler(userManagerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldMatchSnapshot();
        }
    }
}
