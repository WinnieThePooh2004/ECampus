using System.Net;
using System.Security.Claims;
using IdentityServer4.Extensions;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Auth;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Auth;

namespace UniversityTimetable.Domain.Auth;

public class AuthenticationService : IAuthenticationService
{
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(ILogger<AuthenticationService> logger)
    {
        _logger = logger;
    }

    public void VerifyUser(ClaimsPrincipal user)
    {
        if (!user.IsAuthenticated())
        {
            _logger.LogAndThrowException(new DomainException(HttpStatusCode.Unauthorized));
            return;
        }

        var id = user.Claims.FirstOrDefault(claim => claim.Type == CustomClaimTypes.Id)?.Value;
        if (id is null)
        {
            _logger.LogAndThrowException(new DomainException(HttpStatusCode.Unauthorized));
        }
    }
}