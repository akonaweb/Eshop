using Eshop.Infrastructure;
using Eshop.Persistence;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Eshop.WebApi.Infrastructure
{
    [ExcludeFromCodeCoverage] // TODO - create unit tests
    public class TokenManager : ITokenManager
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IDateTimeProvider dateTimeProvider;

        public TokenManager(UserManager<ApplicationUser> userManager, IConfiguration configuration, IDateTimeProvider dateTimeProvider)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public async Task<TokensResponse> GetTokens(ApplicationUser user)
        {
            var (accessToken, accessTokenExpirationDate) = await GenerateAccessToken(user);
            var (refreshToken, refreshTokenExpirationDate) = GenerateRefreshToken();

            user.UpdateRefreshToken(refreshToken, refreshTokenExpirationDate);
            await userManager.UpdateAsync(user);

            return new TokensResponse(accessToken, accessTokenExpirationDate, refreshToken, refreshTokenExpirationDate);
        }

        public class TokensResponse
        {
            public TokensResponse(string accessToken, DateTime accessTokenExpirationDate, string refreshToken, DateTime refreshTokenExpirationDate)
            {
                if (string.IsNullOrWhiteSpace(accessToken))
                    throw new ArgumentNullException(nameof(accessToken));

                if (string.IsNullOrWhiteSpace(refreshToken))
                    throw new ArgumentNullException(nameof(refreshToken));

                AccessToken = accessToken;
                AccessTokenExpirationDate = accessTokenExpirationDate;
                RefreshToken = refreshToken;
                RefreshTokenExpirationDate = refreshTokenExpirationDate;
            }

            public string AccessToken { get; }
            public DateTime AccessTokenExpirationDate { get; }
            public string RefreshToken { get; }
            public DateTime RefreshTokenExpirationDate { get; }
        }

        private async Task<(string AccessToken, DateTime ExpirationDate)> GenerateAccessToken(ApplicationUser user)
        {
            var expirationDate = dateTimeProvider.Now.AddMinutes(30);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email!)
            };

            var roles = await userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var authConfiguration = new AuthConfiguration(configuration);

            var token = new JwtSecurityToken(
                signingCredentials: authConfiguration.SigningCredentials,
                issuer: authConfiguration.Issuer,
                audience: authConfiguration.Audience,
                claims: claims,
                expires: expirationDate
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expirationDate);
        }

        private (string RefreshToken, DateTime RefreshTokenExpirationDate) GenerateRefreshToken()
        {
            var expirationDate = dateTimeProvider.Now.AddDays(7);

            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);

            return (Convert.ToBase64String(randomNumber), expirationDate);
        }
    }
}
