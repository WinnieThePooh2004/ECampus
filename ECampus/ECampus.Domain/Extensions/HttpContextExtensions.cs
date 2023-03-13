using System.Security.Claims;
using ECampus.Domain.Auth;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Responses.Auth;

namespace ECampus.Domain.Extensions;

public static class HttpContextExtensions
{
    public static IEnumerable<Claim> CreateClaims(this LoginResponse loginResponse)
        => new List<Claim>
        {
            new(ClaimTypes.Email, loginResponse.Email),
            new(ClaimTypes.Name, loginResponse.Username),
            new(ClaimTypes.Role, loginResponse.Role),
            new(ClaimTypes.Sid, loginResponse.UserId.ToString(), ClaimValueTypes.Integer32),
            new(ClaimTypes.Authentication, loginResponse.Token),
            new(CustomClaimTypes.StudentId, loginResponse.StudentId?.ToString() ?? "0"),
            new(CustomClaimTypes.TeacherId, loginResponse.TeacherId?.ToString() ?? "0"),
            new(CustomClaimTypes.GroupId, loginResponse.GroupId?.ToString() ?? "0")
        };
}