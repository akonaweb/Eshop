using Eshop.Infrastructure;
using Eshop.Persistence;
using Eshop.WebApi.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;

namespace Eshop.WebApi.Features.Users
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IUserContext userContext;
        private readonly UserManager<ApplicationUser> userManager;
        private const string ACCESS_TOKEN_KEY = "accessToken";
        private const string REFRESH_TOKEN_KEY = "refreshToken";

        public UserController(IMediator mediator, IUserContext userContext, UserManager<ApplicationUser> userManager)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        // TODO - move to Features/Mediatr + tests
        [HttpGet("me")]
        [ProducesResponseType(typeof(MeResponseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<MeResponseDto>> Me()
        {
            var userId = userContext.GetUserId();
            var user = await userManager.FindByIdAsync(userId.ToString()!);

            if (user == null)
                return Ok(null);

            var roles = await userManager.GetRolesAsync(user);

            var accessTokenExpirationDate = GetAccessTokenExpirationDate();
            return Ok(new MeResponseDto { Id = user.Id, Email = user.Email!, Role = roles.FirstOrDefault()!, AccessTokenExpirationDate = accessTokenExpirationDate });
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(RegisterUserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegisterUserResponseDto>> Register(RegisterUserRequestDto request)
        {
            var result = await mediator.Send(new RegisterUser.Command(request));
            return Ok(result);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto request)
        {
            var result = await mediator.Send(new LoginUser.Command(request));
            var cookies = new CookiesDto
            {
                AccessToken = result.AccessToken,
                AccessTokenExpirationDate = result.AccessTokenExpirationDate,
                RefreshToken = result.RefreshToken,
                RefreshTokenExpirationDate = result.RefreshTokenExpirationDate
            };

            SetCookies(cookies);

            return Ok();
        }

        [HttpPost("logout")]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        public ActionResult Logout()
        {
            Response.Cookies.Delete(ACCESS_TOKEN_KEY);
            Response.Cookies.Delete(REFRESH_TOKEN_KEY);

            return NoContent();
        }

        [HttpGet("refresh-tokens")]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RefreshTokensResponseDto>> RefreshTokens()
        {
            var refreshToken = Request.Cookies[REFRESH_TOKEN_KEY] ?? string .Empty;
            var result = await mediator.Send(new RefreshTokens.Command(refreshToken));

            var cookies = new CookiesDto
            {
                AccessToken = result.AccessToken,
                AccessTokenExpirationDate = result.AccessTokenExpirationDate,
                RefreshToken = result.RefreshToken,
                RefreshTokenExpirationDate = result.RefreshTokenExpirationDate
            };

            SetCookies(cookies);

            return Ok();
        }

        [Authorize]
        [HttpPost("change-password")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> ChangePassword(ChangePasswordRequestDto request)
        {
            await mediator.Send(new ChangePassword.Command(request));
            return Ok("Password changed successfully.");
        }

        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> ForgotPassword(ForgotPasswordRequestDto request)
        {
            var result = await mediator.Send(new ForgotPassword.Command(request));
            return Ok(result);
        }

        [HttpPost("forgot-password-confirm")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> ForgotPasswordConfirm(ForgotPasswordConfirmRequestDto request)
        {
            var result = await mediator.Send(new ForgotPasswordConfirm.Command(request));
            return Ok(result);
        }

        private void SetCookies(CookiesDto dto)
        {
            Response.Cookies.Append(ACCESS_TOKEN_KEY, dto.AccessToken, new CookieOptions
            {
                HttpOnly = true, // // Note: also on FE SSR
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = dto.AccessTokenExpirationDate,
            });

            Response.Cookies.Append(REFRESH_TOKEN_KEY, dto.RefreshToken, new CookieOptions
            {
                HttpOnly = false, // Note: needed only on BE
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = dto.RefreshTokenExpirationDate,
            });
        }
        public class MeResponseDto
        {
            public required string Id { get; set; }
            public required string Email { get; set; }
            public required string Role { get; set; }
            public DateTime AccessTokenExpirationDate { get; set; }
        }

        private DateTime GetAccessTokenExpirationDate()
        {
            Request.Cookies.TryGetValue("accessToken", out var token);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var expUnix = long.Parse(jwtToken.Claims.First(c => c.Type == "exp").Value);
            var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;

            return expirationDate;
        }

        private class CookiesDto
        {
            public required string AccessToken { get; set; }
            public required string RefreshToken { get; set; }
            public DateTime AccessTokenExpirationDate { get; set; }
            public DateTime RefreshTokenExpirationDate { get; set; }
        }
    }
}
