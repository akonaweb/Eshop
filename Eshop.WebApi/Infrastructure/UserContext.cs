﻿using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Eshop.WebApi.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Guid GetUserId()
        {
            var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User is not authenticated.");

            return Guid.Parse(userId);
        }
    }

}
