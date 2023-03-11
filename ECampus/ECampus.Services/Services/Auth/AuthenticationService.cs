using System.Net;
using ECampus.Core.Installers;
using ECampus.Services.Contracts.Services;
using ECampus.Shared.Enums;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Extensions;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;

namespace ECampus.Services.Services.Auth;

[Inject(typeof(IAuthenticationService))]
public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationService(IHttpContextAccessor httpContextAccessor)
    {
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