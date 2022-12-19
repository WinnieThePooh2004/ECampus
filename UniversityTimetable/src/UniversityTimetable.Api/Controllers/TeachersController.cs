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
    public class TeachersController : ControllerBase
    {
        private readonly IService<TeacherDTO, TeacherParameters> _service;

        public TeachersController(IService<TeacherDTO, TeacherParameters> service)
        {
            _service = service;
        }

        // GET: Teachers
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] TeacherParameters parameters)
        {
            return Ok(await _service.GetByParametersAsync(parameters));
        }

        // GET: Teachers/Details/5
        [HttpGet("{id:int?}")]
        public async Task<IActionResult> Get(int? id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Post(TeacherDTO teacher)
        {
            await _service.CreateAsync(teacher);
            return Ok(teacher);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Put(TeacherDTO teacher)
        {
            await _service.UpdateAsync(teacher);
            return Ok(teacher);
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
