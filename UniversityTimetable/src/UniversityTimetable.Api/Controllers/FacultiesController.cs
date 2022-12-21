using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Domain.Auth;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Shared.Interfaces.Services;

namespace UniversityTimetable.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacultiesController : ControllerBase
    {
        private IService<FacultyDto, FacultyParameters> _service;
        public FacultiesController(IService<FacultyDto, FacultyParameters> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] FacultyParameters parameters)
        {
            return Ok(await _service.GetByParametersAsync(parameters));
        }

        [HttpGet("{id:int?}")]
        public async Task<IActionResult> Get(int? id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        [Authorize(UserRole.Admin)]
        public async Task<IActionResult> Post([Bind("Name")] FacultyDto facultacy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _service.CreateAsync(facultacy);
            return Ok(facultacy);
        }

        [HttpPut]
        [Authorize(UserRole.Admin)]
        public async Task<IActionResult> Put(FacultyDto faculty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(faculty);
            }
            await _service.UpdateAsync(faculty);
            return Ok(faculty);
        }

        [HttpDelete("{id:int?}")]
        [Authorize(UserRole.Admin)]
        public async Task<IActionResult> Delete(int? id)
        {
            await _service.DeleteAsync(id);
            return Ok(id);
        }
    }
}
