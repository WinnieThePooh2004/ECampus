using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Pages.Auth;

public class Logout : PageModel
{
    private readonly IAuthRequests _requests;

    public Logout(IAuthRequests requests)
    {
        _requests = requests;
    }
    
    public async Task<IActionResult> OnGetAsync()
    {
        await HttpContext
            .SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        await _requests.LogoutAsync();
        return LocalRedirect(Url.Content("~/"));
    }
}