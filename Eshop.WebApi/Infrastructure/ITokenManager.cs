using Eshop.Persistence;
using static Eshop.WebApi.Infrastructure.TokenManager;

namespace Eshop.WebApi.Infrastructure
{
    public interface ITokenManager
    {
        Task<TokensResponse> GetTokens(ApplicationUser user);
    }
}