using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Services;

namespace UniversityTimetable.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _service;

        public ClassesController(IClassService service)
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

        [HttpPut("Validate")]
        public async Task<IActionResult> Validate(ClassDTO @class)
        {
            return Ok(await _service.ValidateAsync(@class));
        }

        [HttpGet("Teacher/{teacherId}")]
        public async Task<IActionResult> TeacherTimetable(int teacherId)
        {
            var table = await _service.GetTimetableForTeacherAsync(teacherId);
            return Ok(table);
        }

        // GET: Classes/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }


        // GET: Classes/Create
        [HttpPost]
        public async Task<IActionResult> Create(ClassDTO @class)
        {
            return Ok(await _service.CreateAsync(@class));
        }

        [HttpPut]
        public async Task<IActionResult> Edit(ClassDTO @class)
        {
            return Ok(await _service.UpdateAsync(@class));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(id);
        }
    }
}
