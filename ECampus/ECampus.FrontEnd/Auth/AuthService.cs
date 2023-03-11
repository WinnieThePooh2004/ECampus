using System.Security.Claims;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Exceptions.DomainExceptions;
using ECampus.Domain.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ECampus.FrontEnd.Auth;

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
        var claims = user.CreateClaims();

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity), properties);
    }

}