using System.Security.Claims;
using ECampus.Shared.Auth;

namespace ECampus.Shared.Extensions;

public static class HttpContextExtensions
{
    public static IEnumerable<Claim> CreateClaims(LoginResult loginResult)
        => new List<Claim>
        {
            new(ClaimTypes.Email, loginResult.Email),
            new(ClaimTypes.Name, loginResult.Username),
            new(ClaimTypes.Role, loginResult.Role.ToString()),
            new(CustomClaimTypes.Id, loginResult.UserId.ToString(), ClaimValueTypes.Integer32),
            new(CustomClaimTypes.JwtBearer, loginResult.Token),
            new(CustomClaimTypes.StudentId, loginResult.StudentId?.ToString() ?? "-1"),
            new(CustomClaimTypes.TeacherId, loginResult.TeacherId?.ToString() ?? "-1"),
            new(CustomClaimTypes.GroupId, loginResult.GroupId?.ToString() ?? "-1")
        };
}