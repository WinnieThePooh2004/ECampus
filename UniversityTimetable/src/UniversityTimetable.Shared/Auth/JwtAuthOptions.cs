using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace UniversityTimetable.Shared.Auth;

public static class JwtAuthOptions
{
    public const string Issuer = "MyAuthServer";
    public const string Audience = "MyAuthClient";
    private const string Key = "mysupersecret_secretkey!123";
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
}