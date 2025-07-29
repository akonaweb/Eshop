using Eshop.WebApi.Features.Users;
using FluentValidation.TestHelper;

namespace Eshop.WebApi.Tests.Features.Users.ValidatorTests
{
    public class LoginUserValidatorTests
    {
        private LoginUser.Validator validator = null!;

        [SetUp]
        public void Setup()
        {
            validator = new LoginUser.Validator();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void LoginUserValidator_EmailNotNullOrEmpty(string? email)
        {
            var request = new LoginRequestDto { Email = email!, Password = "Valid!Password123" };
            var command = new LoginUser.Command(request);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Email);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void LoginUserValidator_PasswordNotNullOrEmpty(string? passsword)
        {
            var request = new LoginRequestDto { Email = "valid@email.com", Password = passsword! };
            var command = new LoginUser.Command(request);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Password);
        }
    }
}