using ECampus.Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace ECampus.WebApi.Metadata;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class AuthorizedAttribute : AuthorizeAttribute
{
    public AuthorizedAttribute(params UserRole[] roles)
    {
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        Roles = string.Join(",", roles);
    }

    public AuthorizedAttribute()
    {
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
    }
}