using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Shared.DataTransferObjects;
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

        // GET: Auditories
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] AuditoryParameters parameters)
        {
            return Ok(await _service.GetByParametersAsync(parameters));
        }

        // GET: Auditories/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuditoryDTO auditory)
        {
            if (!ModelState.IsValid)
            {
                return Ok(auditory);
            }
            await _service.CreateAsync(auditory);
            return Ok(auditory);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(AuditoryDTO auditory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(auditory);
            }
            await _service.UpdateAsync(auditory);
            return Ok(auditory);
        }

        // POST: Auditories/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            await _service.DeleteAsync(id);
            return Ok(id);
        }
    }
}
