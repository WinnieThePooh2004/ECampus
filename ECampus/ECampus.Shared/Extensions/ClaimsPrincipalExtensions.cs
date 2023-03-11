using System.Security.Claims;
using ECampus.Shared.Auth;

namespace ECampus.Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int? GetId(this ClaimsPrincipal user)
    {
        var idClaim = user.FindFirst(CustomClaimTypes.Id);
        if (idClaim is null)
        {
            return null;
        }

        if (!int.TryParse(idClaim.Value, out var id))
        {
            return null;
        }

        return id;
    }
}