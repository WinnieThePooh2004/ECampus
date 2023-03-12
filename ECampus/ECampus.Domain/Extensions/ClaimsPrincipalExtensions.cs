using System.Security.Claims;

namespace ECampus.Domain.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int? GetId(this ClaimsPrincipal user)
    {
        var idClaim = user.FindFirst(ClaimTypes.Sid);
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