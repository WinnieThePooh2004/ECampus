using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using UniversityTimetable.Shared.Enums;

namespace UniversityTimetable.Domain.Auth;

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

