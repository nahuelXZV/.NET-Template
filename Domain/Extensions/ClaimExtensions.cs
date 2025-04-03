using System.Security.Claims;
using System.Security.Principal;

namespace Domain.Extensions;

public static class ClaimExtensions
{
    public static string GetClaimValue(this IPrincipal currentPrincipal, string key)
    {
        try
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            if (identity == null) return null;

            var claim = identity.Claims.FirstOrDefault(c => c.Type == key);
            return claim?.Value;
        }
        catch (Exception ex)
        {
            string message = ex.Message;
            return null;
        }
    }
}
