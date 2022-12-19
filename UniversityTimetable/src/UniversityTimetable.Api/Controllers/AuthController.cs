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
        public async Task<IActionResult> Logout(string username)
        {
            await _authService.Logout(username);
            return Ok();
        }
    }
}
