using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Services;

namespace UniversityTimetable.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimetableController : ControllerBase
    {
        private readonly IClassService _service;

        public TimetableController(IClassService service)
        {
            _service = service;
        }

        [HttpGet("Auditory/{auditoryId}")]
        public async Task<IActionResult> AuditoryTimetable(int auditoryId)
        {
            var table = await _service.GetTimetableForAuditoryAsync(auditoryId);
            return Ok(table);
        }

        [HttpGet("Group/{groupId}")]
        public async Task<IActionResult> GroupTimetable(int groupId)
        {
            var table = await _service.GetTimetableForGroupAsync(groupId);
            return Ok(table);
        }

        [HttpGet("Teacher/{teacherId}")]
        public async Task<IActionResult> TeacherTimetable(int teacherId)
        {
            var table = await _service.GetTimetableForTeacherAsync(teacherId);
            return Ok(table);
        }

        // GET: Classes/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }


        // GET: Classes/Create
        [HttpPost]
        public async Task<IActionResult> Post(ClassDTO @class)
        {
            return Ok(await _service.CreateAsync(@class));
        }

        [HttpPut]
        public async Task<IActionResult> Put(ClassDTO @class)
        {
            return Ok(await _service.UpdateAsync(@class));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(id);
        }

        [HttpPut("Validate")]
        public async Task<IActionResult> Validate(ClassDTO model)
        {
            return Ok(await _service.ValidateAsync(model));
        }
    }
}
