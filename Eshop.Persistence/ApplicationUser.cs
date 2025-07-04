using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace Eshop.Persistence
{
    // TODO: Eventually we should test this Entity as well - we do not write however test for Persistence now.
    // We do not want to move it into Domain as it is having reference to the EF. So new test project is needed.
    [ExcludeFromCodeCoverage] 
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
