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
    public class DepartmentsController : ControllerBase
    {
        private readonly IService<DepartmentDTO, DepartmentParameters> _service;
        public DepartmentsController(IService<DepartmentDTO, DepartmentParameters> service)
        {
            _service = service;
        }

        // GET: Departments
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DepartmentParameters parameters)
        {
            return Ok(await _service.GetByParametersAsync(parameters));
        }

        // GET: Departments/Details/5
        [HttpGet("{id:int?}")]
        public async Task<IActionResult> Get(int? id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(UserRole.Admin)]
        public async Task<IActionResult> Post(DepartmentDTO department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _service.CreateAsync(department);
            return Ok(department);
        }

        [HttpPut]
        [Authorize(UserRole.Admin)]
        public async Task<IActionResult> Put(DepartmentDTO department)
        {
            await _service.UpdateAsync(department);
            return Ok(department);
        }

        // GET: Departments/Delete/5
        [HttpDelete("{id:int?}")]
        [Authorize(UserRole.Admin)]
        public async Task<IActionResult> Delete(int? id)
        {
            await _service.DeleteAsync(id);
            return Ok(id);
        }
    }
}
