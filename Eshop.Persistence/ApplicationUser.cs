using Microsoft.AspNetCore.Identity;

namespace Eshop.Persistence
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiryDate { get; private set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private ApplicationUser() { } // private ctor needed for a persistence - Entity Framework
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public ApplicationUser(string email)
        {
            UserName = email;
            Email = email;
            RefreshToken = null;
            RefreshTokenExpiryDate = null;
        }

        public void UpdateRefreshToken(string refreshToken)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(7);
        }
    }
}
