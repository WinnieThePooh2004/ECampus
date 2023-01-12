using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using UniversityTimetable.Shared.Auth;

namespace UniversityTimetable.Shared.Extensions;

public static class HttpContextExtensions
{
    public static IEnumerable<Claim> CreateClaims(LoginResult loginResult)
        => new List<Claim>
        {
            new(ClaimTypes.Email, loginResult.Email),
            new(ClaimTypes.Name, loginResult.Username),
            new(ClaimTypes.Role, loginResult.Role.ToString()),
            new(CustomClaimTypes.Id, loginResult.UserId.ToString(), ClaimValueTypes.Integer32),
            new(CustomClaimTypes.JwtBearer, loginResult.Token)
        };
}