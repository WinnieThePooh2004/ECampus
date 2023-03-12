using System.Security.Claims;
using ECampus.Domain.Auth;
using ECampus.Domain.DataTransferObjects;

namespace ECampus.Domain.Extensions;

public static class HttpContextExtensions
{
    public static IEnumerable<Claim> CreateClaims(this LoginResult loginResult)
        => new List<Claim>
        {
            new(ClaimTypes.Email, loginResult.Email),
            new(ClaimTypes.Name, loginResult.Username),
            new(ClaimTypes.Role, loginResult.Role),
            new(ClaimTypes.Sid, loginResult.UserId.ToString(), ClaimValueTypes.Integer32),
            new(ClaimTypes.Authentication, loginResult.Token),
            new(CustomClaimTypes.StudentId, loginResult.StudentId?.ToString() ?? "0"),
            new(CustomClaimTypes.TeacherId, loginResult.TeacherId?.ToString() ?? "0"),
            new(CustomClaimTypes.GroupId, loginResult.GroupId?.ToString() ?? "0")
        };
}