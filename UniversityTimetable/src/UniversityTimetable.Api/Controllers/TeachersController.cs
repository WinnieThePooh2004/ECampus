using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Shared.DataTransferObjects;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post(TeacherDTO teacher)
        {
            await _service.CreateAsync(teacher);
            return Ok(teacher);
        }

        [HttpPut]
        public async Task<IActionResult> Put(TeacherDTO teacher)
        {
            await _service.UpdateAsync(teacher);
            return Ok(teacher);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            await _service.DeleteAsync(id);
            return Ok(id);
        }
    }
}
