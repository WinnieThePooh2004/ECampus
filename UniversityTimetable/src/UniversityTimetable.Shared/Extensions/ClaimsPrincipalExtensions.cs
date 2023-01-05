using System.Security.Claims;
using UniversityTimetable.Shared.Auth;

namespace UniversityTimetable.Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int? GetId(this ClaimsPrincipal user)
    {
        var idClaim = user.Claims.FirstOrDefault(claim => claim.Type == CustomClaimTypes.Id);
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

    public static string? GetBearer(this ClaimsPrincipal user) => user.Claims.FirstOrDefault(claim => claim.Type == CustomClaimTypes.JwtBearer)?.Value;
}