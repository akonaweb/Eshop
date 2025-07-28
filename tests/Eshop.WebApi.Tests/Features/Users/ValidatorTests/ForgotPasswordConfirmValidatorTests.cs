using Eshop.WebApi.Features.Users;
using FluentValidation.TestHelper;
using static Eshop.WebApi.Features.Users.ForgotPasswordConfirm;

namespace Eshop.WebApi.Tests.Features.Users.ValidatorTests
{
    public class ForgotPasswordConfirmValidatorTests
    {
        private ForgotPasswordConfirm.Validator validator = null!;

        [SetUp]
        public void SetUp()
        {
            validator = new ForgotPasswordConfirm.Validator();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ForgotPasswordConfirm_EmailNotNullOrEmpty(string? email)
        {
            var request = new ForgotPasswordConfirmRequestDto { Email = email!, ResetCode = "valid-reset-code", NewPassword = "valid!123" };
            var command = new ForgotPasswordConfirm.Command(request);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Email);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ForgotPasswordConfirm_ResetCode_NotEmpty(string? resetCode)
        {
            var request = new ForgotPasswordConfirmRequestDto { Email = "test@test.com", ResetCode = resetCode!, NewPassword = "valid!123" };
            var command = new ForgotPasswordConfirm.Command(request);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.ResetCode);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ForgotPasswordConfirm_NewPassword_NotEmpty(string? newPassword)
        {
            var request = new ForgotPasswordConfirmRequestDto { Email = "test@test.com", ResetCode = "valid-reset-code", NewPassword = newPassword! };
            var command = new ForgotPasswordConfirm.Command(request);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.NewPassword);
        }
    }
}
