using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using UniversityTimetable.Shared.Auth;
using UniversityTimetable.Shared.DataTransferObjects;
namespace UniversityTimetable.Shared.Extensions;

public static class HttpContextExtensions
{
    public static async Task SignInAsync(this HttpContext context, UserDto user)
    {
        await context.SignInAsync(user, new AuthenticationProperties());
    }

    public static async Task SignInAsync(this HttpContext context, UserDto user, AuthenticationProperties properties)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(CustomClaimTypes.Id, user.Id.ToString(), ClaimValueTypes.Integer32)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity), properties);

    }
}