using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Services;

namespace UniversityTimetable.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("{id:int?}")]
        public async Task<IActionResult> Get(int? id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(UserDTO user)
        {
            return Ok(await _service.CreateAsync(user));
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Put(UserDTO user)
        {
            return Ok(await _service.UpdateAsync(user));
        }

        [HttpDelete("{id:int?}")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int? id)
        {
            await _service.DeleteAsync(id);
            return Ok(id);
        }

        [HttpPut("Validate")]
        public async Task<IActionResult> Validate(UserDTO user)
        {
            return Ok(await _service.ValidateAsync(user));
        }
    }
}
