using Eshop.Persistence;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Eshop.WebApi.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class TokenManager : ITokenManager
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;

        public TokenManager(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<TokensResponse> GetTokens(ApplicationUser user)
        {
            var accessToken = await GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();
            user.UpdateRefreshToken(refreshToken);
            await userManager.UpdateAsync(user);

            return new TokensResponse(accessToken, refreshToken);
        }

        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
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
                expires: DateTime.UtcNow.AddMinutes(30) // NOTE: we should use DateTimeProvider
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public class TokensResponse
        {
            public TokensResponse(string accessToken, string refreshToken)
            {
                if (string.IsNullOrWhiteSpace(accessToken))
                    throw new ArgumentNullException(nameof(accessToken));

                if (string.IsNullOrWhiteSpace(refreshToken))
                    throw new ArgumentNullException(nameof(refreshToken));

                AccessToken = accessToken;
                RefreshToken = refreshToken;
            }

            public string AccessToken { get; }
            public string RefreshToken { get; }
        }
    }
}
