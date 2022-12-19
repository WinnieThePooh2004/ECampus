using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Shared.Interfaces.Services;

namespace UniversityTimetable.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriesController : ControllerBase
    {
        private readonly IService<AuditoryDTO, AuditoryParameters> _service;

        public AuditoriesController(IService<AuditoryDTO, AuditoryParameters> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] AuditoryParameters parameters)
        {
            return Ok(await _service.GetByParametersAsync(parameters));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int? id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Post(AuditoryDTO auditory)
        {
            if (!ModelState.IsValid)
            {
                return Ok(auditory);
            }
            await _service.CreateAsync(auditory);
            return Ok(auditory);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Put(AuditoryDTO auditory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(auditory);
            }
            await _service.UpdateAsync(auditory);
            return Ok(auditory);
        }

        [HttpDelete("{id:int?}")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Delete(int? id)
        {
            await _service.DeleteAsync(id);
            return Ok(id);
        }
    }
}
