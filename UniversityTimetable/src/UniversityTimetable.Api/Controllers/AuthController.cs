using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Domain.Auth;
using UniversityTimetable.Shared.DataTransferObjects;
using IAuthorizationService = UniversityTimetable.Shared.Interfaces.Auth.IAuthorizationService;

namespace UniversityTimetable.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            return Ok(await _authorizationService.Login(login));
        }

        [HttpDelete]
        [Route("logout")]
        [Authorized]
        public async Task<IActionResult> Logout()
        {
            await _authorizationService.Logout();
            return NoContent();
        }
    }
}
