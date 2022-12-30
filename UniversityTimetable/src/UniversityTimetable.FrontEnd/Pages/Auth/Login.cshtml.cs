using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.Extensions;

namespace UniversityTimetable.FrontEnd.Pages.Auth;

[AllowAnonymous]
public class LoginModel : PageModel
{
    public string ReturnUrl { get; set; } = string.Empty;
    private readonly IAuthRequests _authRequests;

    public LoginModel(IAuthRequests requests)
    {
        _authRequests = requests;
    }
    public async Task<IActionResult> OnGetAsync(string email, string password)
    {
        var returnUrl = Url.Content("~/");
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            RedirectUri = Request.Host.Value
        };
        var login = new LoginDto { Email = email, Password = password };
        var user = await _authRequests.LoginAsync(login);
        await HttpContext.SignInAsync(user, authProperties);

        return LocalRedirect(returnUrl);
    }
}