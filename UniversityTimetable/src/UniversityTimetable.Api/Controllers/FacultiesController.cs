using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Shared.Interfaces.Services;

namespace UniversityTimetable.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacultiesController : ControllerBase
    {
        private IService<FacultyDTO, FacultyParameters> _service;
        public FacultiesController(IService<FacultyDTO, FacultyParameters> service)
        {
            _service = service;
        }

        // GET: Facultacies
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] FacultyParameters parameters)
        {
            return Ok(await _service.GetByParametersAsync(parameters));
        }

        // GET: Facultacies/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        // POST: Facultacies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Post([Bind("Name")] FacultyDTO facultacy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _service.CreateAsync(facultacy);
            return Ok(facultacy);
        }

        // POST: Facultacies/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        public async Task<IActionResult> Put(FacultyDTO facultacy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(facultacy);
            }
            await _service.UpdateAsync(facultacy);
            return Ok(facultacy);
        }

        // POST: Facultacies/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            await _service.DeleteAsync(id);
            return Ok(id);
        }
    }
}
