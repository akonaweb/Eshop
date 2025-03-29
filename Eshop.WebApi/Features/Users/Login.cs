using Eshop.Persistence;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace Eshop.WebApi.Features.Users
{
    public class Login
    {
        public record Command(LoginRequest Request) : IRequest<LoginResponse>;

        public class Handler : IRequestHandler<Command, LoginResponse>
        {
            private readonly UserManager<ApplicationUser> userManager;
            private readonly SignInManager<ApplicationUser> signInManager;
            private readonly IConfiguration configuration;

            public Handler(UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           IConfiguration configuration)
            {
                this.userManager = userManager;
                this.signInManager = signInManager;
                this.configuration = configuration;
            }

            public async Task<LoginResponse> Handle(Command command, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(command.Request.Email);
                if (user is null)
                    throw new UnauthorizedException("Invalid credentials");

                var result = await signInManager.PasswordSignInAsync(user, command.Request.Password, false, false);
                if (!result.Succeeded)
                    throw new UnauthorizedException("Invalid credentials");

                var tokenManager = new TokenManager(userManager, configuration);
                var tokens = await tokenManager.GetTokens(user);

                return new LoginResponse
                {
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken
                };
            }
        }

        public class LoginResponse
        {
            public required string AccessToken { get; set; }
            public required string RefreshToken { get; set; }
        }
    }
}
