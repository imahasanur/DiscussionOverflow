
using System.Security.Claims;

namespace DiscussionOverflow.Infrastructure.Membership
{
    public interface ITokenService
    {
        Task<string> GetJwtToken(IList<Claim> claims, string key, string issuer, string audience);
    }
}