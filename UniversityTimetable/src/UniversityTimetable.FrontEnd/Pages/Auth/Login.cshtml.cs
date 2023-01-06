using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UniversityTimetable.FrontEnd.Auth;

namespace UniversityTimetable.FrontEnd.Pages.Auth;

[AllowAnonymous]
public class LoginModel : PageModel
{
    public string ReturnUrl { get; set; } = string.Empty;
    private readonly IAuthService _authService;

    public LoginModel(IAuthService service)
    {
        _authService = service;
    }
    
    public async Task<IActionResult> OnGetAsync(string email, string password)
    {
        var returnUrl = Url.Content("~/");
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            RedirectUri = Request.Host.Value
        };
        await _authService.Login(email, password, authProperties);
        return LocalRedirect(returnUrl);
    }
}