using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Authorization;

namespace UniversityTimetable.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            return Ok(await _authService.Login(login, HttpContext));
        }

        [HttpDelete]
        [Route("logout")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout(HttpContext);
            return Ok();
        }
    }
}
