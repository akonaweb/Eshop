using Eshop.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Eshop.WebApi.Features.Users
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IDateTimeProvider dateTimeProvider;

        private const string ACCESS_TOKEN_KEY = "accessToken";
        private const string REFRESH_TOKEN_KEY = "refreshToken";

        public UserController(IMediator mediator, IDateTimeProvider dateTimeProvider)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
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
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto request)
        {
            var result = await mediator.Send(new LoginUser.Command(request));
            var cookies = new CookiesDto
            {
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken,
                AccessTokenExpirationDate = dateTimeProvider.Now.AddMinutes(30),
                RefreshTokenExpirationDate = dateTimeProvider.Now.AddDays(7)
            };

            SetCookies(cookies);

            return Ok(result);
        }

        [HttpPost("logout")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public ActionResult Logout()
        {
            Response.Cookies.Delete(ACCESS_TOKEN_KEY);
            Response.Cookies.Delete(REFRESH_TOKEN_KEY);

            return NoContent();
        }

        [HttpGet("refresh-tokens")]
        [ProducesResponseType(typeof(RefreshTokensResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RefreshTokensResponseDto>> RefreshTokens()
        {
            var refreshToken = Request.Cookies[REFRESH_TOKEN_KEY] ?? string.Empty;
            var result = await mediator.Send(new RefreshTokens.Command(refreshToken));

            var cookies = new CookiesDto
            {
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken,
                AccessTokenExpirationDate = dateTimeProvider.Now.AddMinutes(30),
                RefreshTokenExpirationDate = dateTimeProvider.Now.AddDays(7)
            };

            SetCookies(cookies);

            return Ok(result);
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
                HttpOnly = false, // Needed for JavaScript
                Secure = true,
                SameSite = SameSiteMode.None, // TODO: here we should use Strict in case same domain.
                Expires = dto.AccessTokenExpirationDate, // NOTE: coming from TokenManager
            });

            Response.Cookies.Append(REFRESH_TOKEN_KEY, dto.RefreshToken, new CookieOptions
            {
                HttpOnly = true, // It will be not needed on FE
                Secure = true,
                SameSite = SameSiteMode.None, // NOTE: can be strict as it will be refreshed only here in BE
                Expires = dto.RefreshTokenExpirationDate, // NOTE: coming from TokenManager
            });
        }

        internal class CookiesDto
        {
            public required string AccessToken { get; set; }
            public required string RefreshToken { get; set; }
            public DateTime AccessTokenExpirationDate { get; set; }
            public DateTime RefreshTokenExpirationDate { get; set; }
        }
    }
}
