using Eshop.WebApi.Features.Users;
using FluentValidation.TestHelper;

namespace Eshop.WebApi.Tests.Features.Users.ValidatorTests
{
    public class ForgotPasswordValidatorTests
    {
        private ForgotPassword.Validator validator = null!;

        [SetUp]
        public void SetUp()
        {
            validator = new ForgotPassword.Validator();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ForgotPassword_EmailNotNullOrEmpty(string? email)
        {
            var request = new ForgotPasswordRequestDto {  Email = email! };
            var command = new ForgotPassword.Command(request);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Email);
        }
    }
}
