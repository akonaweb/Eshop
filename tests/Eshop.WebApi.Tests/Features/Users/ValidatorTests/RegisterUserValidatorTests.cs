using Eshop.WebApi.Features.Users;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Eshop.WebApi.Tests.Features.Users.ValidatorTests
{
    public class RegisterUserValidatorTests
    {
        private RegisterUser.Handler.Validator validator = null!;

        [SetUp]
        public void Setup()
        {
            validator = new RegisterUser.Handler.Validator();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void RegisterUserValidator_EmailNotNullOrEmpty(string? email)
        {
            var request = new RegisterRequest { Email = email ?? "", Password = "ValidPass123" };
            var command = new RegisterUser.Command(request);

            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Email);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void RegisterUserValidator_PasswordNotNullOrEmpty(string? password)
        {
            var request = new RegisterRequest { Email = "valid@example.com", Password = password ?? "" };
            var command = new RegisterUser.Command(request);

            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Password);
        }

        [TestCase("12345678")]
        [TestCase("Abcd1234")]
        public void RegisterUserValidator_PasswordLengthGreaterThan7(string password)
        {
            var request = new RegisterRequest { Email = "valid@example.com", Password = password };
            var command = new RegisterUser.Command(request);

            var result = validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Request.Password.Length);
        }

        [TestCase("1234567")]
        [TestCase("abc")]
        public void RegisterUserValidator_PasswordTooShort(string password)
        {
            var request = new RegisterRequest { Email = "valid@example.com", Password = password };
            var command = new RegisterUser.Command(request);

            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Password.Length);
        }
    }
}
