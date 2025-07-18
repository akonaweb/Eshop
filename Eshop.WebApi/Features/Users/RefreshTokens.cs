using Eshop.Infrastructure;
using Eshop.Persistence;
using Eshop.WebApi.Exceptions;
using Eshop.WebApi.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Eshop.WebApi.Features.Users
{
    public class RefreshTokens
    {
        public record Command(string RefreshToken) : IRequest<RefreshTokensResponseDto>;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.RefreshToken).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, RefreshTokensResponseDto>
        {
            private readonly ITokenManager tokenManager;
            private readonly IDateTimeProvider dateTimeProvider;
            private readonly UserManager<ApplicationUser> userManager;

            public Handler(ITokenManager tokenManager, IDateTimeProvider dateTimeProvider, UserManager<ApplicationUser> userManager)
            {
                this.tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
                this.dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
                this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            }

            public async Task<RefreshTokensResponseDto> Handle(Command command, CancellationToken cancellationToken)
            {
                var user = await userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == command.RefreshToken);
                if (user == null || user.RefreshTokenExpiryDate <= dateTimeProvider.Now)
                    throw new UnauthorizedException("Invalid or expired refresh token.");

                var token = await tokenManager.GetTokens(user);

                return new RefreshTokensResponseDto
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken
                };
            }
        }

        public class RefreshTokensResponseDto
        {
            public required string AccessToken { get; set; }
            public required string RefreshToken { get; set; }
        }
    }
}