using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using UniversityTimetable.Shared.Enums;

namespace UniversityTimetable.Domain.Auth;

public class AuthorizedAttribute : AuthorizeAttribute
{
    public AuthorizedAttribute(params UserRole[] roles)
    {
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme;
        Roles = string.Join(",", roles);
        
    }
}

