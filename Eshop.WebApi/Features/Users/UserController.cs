using Eshop.Persistence;
using Eshop.WebApi.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Eshop.WebApi.Features.Users
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUserContext userContext;
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;

        public UserController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IUserContext userContext,
                              IConfiguration configuration,
                              IMediator mediator)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            this.userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            var result = await mediator.Send(new RegisterUser.Command(model));
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
                return Unauthorized("Invalid credentials");

            var tokenManager = new TokenManager(userManager, configuration);
            var token = await tokenManager.GetTokens(user);
            return Ok(token);

            //var response = await mediator.Send(new Login.Command(model));
            //return Ok(response);
        }

        [HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshToken(string oldRefreshToken)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == oldRefreshToken);

            if (user == null || user.RefreshTokenExpiryDate <= DateTime.UtcNow)
                return Unauthorized("Invalid or expired refresh token.");

            var tokenManager = new TokenManager(userManager, configuration);
            var token = await tokenManager.GetTokens(user);
            return Ok(token);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest model)
        {
            var userId = userContext.GetUserId();
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null) return NotFound();

            var changeResult = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!changeResult.Succeeded)
            {
                var errors = changeResult.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }

            return Ok("Password changed successfully.");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("User not found."); // Here you should send Ok() result to not show that user exists in DB.

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            //var resetUrl = $"{configuration["FrontendUrl"]}/reset-password?token={WebUtility.UrlEncode(token)}&email={user.Email}";
            //await emailSender.SendEmailAsync(user.Email, "Reset Password", $"Click <a href='{resetUrl}'>here</a> to reset your password.");

            return Ok(new { token }); // "Reset password link sent."
        }

        [HttpPost("forgot-password-confirm")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("Invalid request.");

            var result = await userManager.ResetPasswordAsync(user, model.ResetCode, model.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Password has been reset.");
        }

    }

    public class ChangePasswordRequest
    {
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}