using ECampus.FrontEnd.Requests.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECampus.FrontEnd.Pages.Auth;

public class Logout : PageModel
{
    private readonly IAuthRequests _requests;

    public Logout(IAuthRequests requests)
    {
        _requests = requests;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await _requests.LogoutAsync();
        await HttpContext
            .SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        return LocalRedirect(Url.Content("~/"));
    }
}