using Eshop.WebApi.Features.Users;
using FluentValidation.TestHelper;

namespace Eshop.WebApi.Tests.Features.Users.ValidatorTests
{
    public class ChangePasswordValidatorTests
    {
        private ChangePassword.Validator validator = null!;

        [SetUp]
        public void SetUp()
        {
            validator = new ChangePassword.Validator();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ChangePasswordUser_CurrentPasswordNotNullOrEmpty(string? currentPassword)
        {
            var request = new ChangePasswordRequestDto { CurrentPassword = currentPassword!, NewPassword = "Valid!Password123" };
            var command = new ChangePassword.Command(request);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.CurrentPassword);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ChangePasswordUser_NewPasswordNotNullOrEmpty(string? newPassword)
        {
            var request = new ChangePasswordRequestDto { CurrentPassword = "Valid!Password123", NewPassword = newPassword! };
            var command = new ChangePassword.Command(request);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.NewPassword);
        }
    }
}
