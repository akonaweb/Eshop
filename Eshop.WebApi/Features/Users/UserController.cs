using Eshop.Infrastructure;
using Eshop.Persistence;
using Eshop.WebApi.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly IUserContext userContext;
        private readonly UserManager<ApplicationUser> userManager;
        private const string ACCESS_TOKEN_KEY = "accessToken";
        private const string REFRESH_TOKEN_KEY = "refreshToken";

        public UserController(IMediator mediator, IDateTimeProvider dateTimeProvider, IUserContext userContext, UserManager<ApplicationUser> userManager)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
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

            return Ok(new MeResponseDto { Id = user.Id, Email = user.Email!, Role = roles.FirstOrDefault()! });
        }

        public class MeResponseDto
        {
            public required string Id { get; set; }
            public required string Email { get; set; }
            public required string Role { get; set; }
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
            var refreshToken = Request.Cookies[REFRESH_TOKEN_KEY] ?? string .Empty;
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
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = dto.AccessTokenExpirationDate,
            });

            Response.Cookies.Append(REFRESH_TOKEN_KEY, dto.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = dto.RefreshTokenExpirationDate,
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
