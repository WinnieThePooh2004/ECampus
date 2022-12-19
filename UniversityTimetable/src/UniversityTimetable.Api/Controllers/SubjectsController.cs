using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly IService<SubjectDTO, SubjectParameters> _service;
        public SubjectsController(IService<SubjectDTO, SubjectParameters> service)
        {
            _service = service;
        }

        // GET: Subjects
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] SubjectParameters parameters)
        {
            return Ok(await _service.GetByParametersAsync(parameters));
        }

        // GET: Subjects/Details/5
        [HttpGet("{id:int?}")]
        public async Task<IActionResult> Get(int? id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Post(SubjectDTO subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _service.CreateAsync(subject);
            return Ok(subject);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Put(SubjectDTO subject)
        {
            await _service.UpdateAsync(subject);
            return Ok(subject);
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
