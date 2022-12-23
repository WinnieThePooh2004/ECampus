using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Domain.Auth;
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
        private readonly IParametersService<SubjectDto, SubjectParameters> _service;
        public SubjectsController(IParametersService<SubjectDto, SubjectParameters> service)
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
        [Authorized(UserRole.Admin)]
        public async Task<IActionResult> Post(SubjectDto subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _service.CreateAsync(subject);
            return Ok(subject);
        }

        [HttpPut]
        [Authorized(UserRole.Admin)]
        public async Task<IActionResult> Put(SubjectDto subject)
        {
            await _service.UpdateAsync(subject);
            return Ok(subject);
        }

        [HttpDelete("{id:int?}")]
        [Authorized(UserRole.Admin)]
        public async Task<IActionResult> Delete(int? id)
        {
            await _service.DeleteAsync(id);
            return Ok(id);
        }
    }
}
