using UniversityTimetable.Shared.Auth;

namespace UniversityTimetable.Tests.Shared;

public static class AuthData
{
    public static JwtAuthOptions DefaultOptions => new()
    {
        Issuer = "MyAuthServer",
        Audience = "MyAuthClient",
        Key = "MySuperSecret_SecretKey!123"
    };
}