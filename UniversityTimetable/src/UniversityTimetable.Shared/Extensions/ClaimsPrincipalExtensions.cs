using System.Security.Claims;
using UniversityTimetable.Shared.Auth;

namespace UniversityTimetable.Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetId(this ClaimsPrincipal user)
    {
        return int.Parse(user.Claims.FirstOrDefault(claim => claim.Type == CustomClaimTypes.Id)?.Value ?? "0");
    }
}