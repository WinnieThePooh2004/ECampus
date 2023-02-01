using System.Security.Claims;
using ECampus.Core.Metadata;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ECampus.FrontEnd.Auth;

[Inject(typeof(IAuthRequests))]
public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthRequests _requests;

    public AuthService(IHttpContextAccessor httpContextAccessor, IAuthRequests requests)
    {
        _httpContextAccessor = httpContextAccessor;
        _requests = requests;
    }

    public async Task Login(string email, string password, AuthenticationProperties properties = default!)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new HttpContextNotFoundExceptions();
        }
        var login = new LoginDto { Email = email, Password = password };
        var user = await _requests.LoginAsync(login);
        var claims = HttpContextExtensions.CreateClaims(user);

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity), properties);
    }

}