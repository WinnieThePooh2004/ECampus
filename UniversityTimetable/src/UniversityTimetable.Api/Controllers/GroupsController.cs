using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Domain.Auth;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IParametersService<GroupDto, GroupParameters> _service;

        public GroupsController(IParametersService<GroupDto, GroupParameters> service)
        {
            _service = service;
        }

        // GET: Groups
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] GroupParameters parameters)
        {
            return Ok(await _service.GetByParametersAsync(parameters));
        }

        // GET: Groups/Details/5
        [HttpGet("{id:int?}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int? id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        [Authorized(UserRole.Admin)]
        public async Task<IActionResult> Post(GroupDto group)
        {
            await _service.CreateAsync(group);
            return Ok(group);
        }

        [HttpPut]
        [Authorized(UserRole.Admin)]
        public async Task<IActionResult> Put(GroupDto group)
        {
            await _service.UpdateAsync(group);
            return Ok(group);
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
