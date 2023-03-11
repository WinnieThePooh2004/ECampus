using System.Security.Claims;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using ECampus.Domain.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ECampus.FrontEnd.Pages.Auth;

public partial class SignUp
{
    [Inject] private IAuthRequests Requests { get; set; } = default!;
    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    private async Task Save(RegistrationDto model)
    {
        var result = await Requests.SignUpAsync(model);
        var claims = result.CreateClaims();
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity), new AuthenticationProperties());    
        NavigationManager.NavigateTo("~/");
    }
}