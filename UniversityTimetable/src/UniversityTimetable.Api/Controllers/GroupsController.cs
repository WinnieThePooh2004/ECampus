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
    public class GroupsController : ControllerBase
    {
        private readonly IService<GroupDto, GroupParameters> _service;

        public GroupsController(IService<GroupDto, GroupParameters> service)
        {
            _service = service;
        }

        // GET: Groups
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GroupParameters parameters)
        {
            return Ok(await _service.GetByParametersAsync(parameters));
        }

        // GET: Groups/Details/5
        [HttpGet("{id:int?}")]
        public async Task<IActionResult> Get(int? id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Post(GroupDto group)
        {
            if (!ModelState.IsValid || group.DepartmentId == 0)
            {
                return BadRequest(group);
            }
            await _service.CreateAsync(group);
            return Ok(group);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Put(GroupDto group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(group);
            }
            await _service.CreateAsync(group);
            return Ok(group);
        }

        // POST: Groups/Delete/5
        [HttpDelete("{id:int?}")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(id);
        }
    }
}
