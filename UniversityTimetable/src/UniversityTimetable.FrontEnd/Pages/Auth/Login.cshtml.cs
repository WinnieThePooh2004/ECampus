using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Pages.Auth
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        public string ReturnUrl { get; set; }
        private readonly IAuthRequests _authRequests;

        public LoginModel(IAuthRequests requests)
        {
            _authRequests = requests;
        }
        public async Task<IActionResult> OnGetAsync(string username, string password)
        {
            var returnUrl = Url.Content("~/");
            var login = new LoginDTO { Email = "vova2004hunko@icloud.com", Password = "Vova1234#" };
            var user = await _authRequests.LoginAsync(login);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = this.Request.Host.Value
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return LocalRedirect(returnUrl);
        }
    }
}