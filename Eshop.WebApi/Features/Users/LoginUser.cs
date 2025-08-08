using Eshop.Persistence;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Eshop.WebApi.Features.Users
{
    public class LoginUser
    {
        public record Command(LoginRequestDto Request) : IRequest<LoginResponseDto>;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Request.Email).NotEmpty();
                RuleFor(x => x.Request.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, LoginResponseDto>
        {
            private readonly UserManager<ApplicationUser> userManager;
            private readonly SignInManager<ApplicationUser> signInManager;
            private readonly ITokenManager tokenManager;

            public Handler(UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           ITokenManager tokenManager)
            {
                this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
                this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
                this.tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            }

            public async Task<LoginResponseDto> Handle(Command command, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(command.Request.Email);
                if (user is null)
                    throw new UnauthorizedException("Invalid credentials.");

                var result = await signInManager.PasswordSignInAsync(user, command.Request.Password, false, false);
                if (!result.Succeeded)
                    throw new UnauthorizedException("Invalid credentials.");

                var tokens = await tokenManager.GetTokens(user);

                return new LoginResponseDto
                {
                    AccessToken = tokens.AccessToken,
                    AccessTokenExpirationDate = tokens.AccessTokenExpirationDate,
                    RefreshToken = tokens.RefreshToken,
                    RefreshTokenExpirationDate = tokens.RefreshTokenExpirationDate
                };
            }
        }
    }

    public sealed class LoginRequestDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public required string AccessToken { get; set; }
        public DateTime AccessTokenExpirationDate { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationDate { get; set; }
    }
}
