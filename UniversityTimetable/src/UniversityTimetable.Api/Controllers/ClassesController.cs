using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Services;

namespace UniversityTimetable.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _service;

        public ClassesController(IClassService service)
        {
            _service = service;
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
