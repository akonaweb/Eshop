using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Eshop.WebApi.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class AuthConfiguration
    {
        public AuthConfiguration(IConfiguration configuration)
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            var secretKey = configuration["Auth:SecretKey"];
            var issuer = configuration["Auth:Issuer"];
            var audience = configuration["Auth:Audience"];

            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException(nameof(secretKey));
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            if (secretKeyBytes.Length < 32)  // Check more then bits/bytes 256/32
                throw new ArgumentException("The key size must be 256 bits (32 bytes) or greater.");
            if (string.IsNullOrEmpty(issuer))
                throw new ArgumentNullException(nameof(issuer));
            if (string.IsNullOrEmpty(audience))
                throw new ArgumentNullException(nameof(audience));

            SigningKey = new SymmetricSecurityKey(secretKeyBytes);
            SigningCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);
            Issuer = issuer;
            Audience = audience;
        }

        public SymmetricSecurityKey SigningKey { get; }
        public SigningCredentials SigningCredentials { get; }
        public string Audience { get; }
        public string Issuer { get; }
    }
}
