using System.Security.Claims;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UniversityTimetable.Shared.Enums;

namespace UniversityTimetable.Domain.Auth;

public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly UserRole _role;

    public AuthorizeAttribute(UserRole role = UserRole.Guest)
    {
        _role = role;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.IsAuthenticated())
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var claimRole = context.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);
        if (claimRole is null || !Enum.TryParse(claimRole.Value, out UserRole role))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (role <= _role)
        {
            context.Result = new ForbidResult($"You must be at least {_role} to access this action");
        }
    }
}