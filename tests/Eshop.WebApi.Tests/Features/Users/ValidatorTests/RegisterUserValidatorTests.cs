﻿using Eshop.WebApi.Features.Users;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Identity.Data;

namespace Eshop.WebApi.Tests.Features.Users.ValidatorTests
{
    public class RegisterUserValidatorTests
    {
        private RegisterUser.Validator validator = null!;

        [SetUp]
        public void Setup()
        {
            validator = new RegisterUser.Validator();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void RegisterUserValidator_EmailNotNullOrEmpty(string? email)
        {
            var request = new RegisterRequest { Email = email!, Password = "Valid!Password123" };
            var command = new RegisterUser.Command(request);

            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Email);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void RegisterUserValidator_PasswordNotNullOrEmpty(string? password)
        {
            var request = new RegisterRequest { Email = "valid@email.com", Password = password! };
            var command = new RegisterUser.Command(request);

            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Request.Password);
        }
    }
}
