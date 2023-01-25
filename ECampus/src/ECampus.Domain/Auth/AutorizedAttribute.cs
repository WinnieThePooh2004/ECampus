using ECampus.Shared.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace ECampus.Domain.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizedAttribute : AuthorizeAttribute
{
    public AuthorizedAttribute(params UserRole[] roles)
    {
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        Roles = string.Join(",", roles);
    }

    public AuthorizedAttribute(UserRole minimalRole)
        : this(Enum.GetValues<UserRole>().Where(role => role >= minimalRole).ToArray())
    {
        
    }

    public AuthorizedAttribute()
    {
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
    }
}