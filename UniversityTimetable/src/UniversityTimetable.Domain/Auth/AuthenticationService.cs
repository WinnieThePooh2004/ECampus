using System.Net;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Auth;

namespace UniversityTimetable.Domain.Auth;

public class AuthenticationService : IAuthenticationService
{
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationService(ILogger<AuthenticationService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public void VerifyUser(int userId)
    {
        var user = _httpContextAccessor.HttpContext?.User
                   ?? throw new HttpContextNotFoundExceptions();
        
        if (!user.IsAuthenticated())
        {
            throw new DomainException(HttpStatusCode.Unauthorized,
                "Cannot verify user than is not authenticated");
        }

        var id = user.GetId();
        if (id is null)
        {
            throw new DomainException(HttpStatusCode.Unauthorized,
                "User claims should have Claim with user id");
        }

        if (id != userId && !user.IsInRole(nameof(UserRole.Admin)))
        {
            throw new DomainException(HttpStatusCode.Forbidden,
                "To do this action you must be registered with user id same as requested or be admin");
        }
    }
}