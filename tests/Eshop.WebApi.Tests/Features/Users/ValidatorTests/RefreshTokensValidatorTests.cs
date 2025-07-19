using Eshop.WebApi.Features.Users;
using FluentValidation.TestHelper;

namespace Eshop.WebApi.Tests.Features.Users.ValidatorTests
{
    public class RefreshTokensValidatorTests
    {
        private RefreshTokens.Validator validator;

        [SetUp]
        public void Setup()
        {
            validator = new RefreshTokens.Validator();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void RefreshTokenValidator_RefreshTokenNotNullOrEmpty(string? refreshToken)
        {
            var command = new RefreshTokens.Command(refreshToken!);
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.RefreshToken);
        }
    }
}
