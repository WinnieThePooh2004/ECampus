using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Domain.Requests.Group;
using ECampus.Domain.Responses.Group;
using ECampus.Services.Contracts.Services;
using ECampus.WebApi.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IParametersService<MultipleGroupResponse, GroupParameters> _service;
    private readonly IBaseService<GroupDto> _baseService;

    public GroupsController(IParametersService<MultipleGroupResponse, GroupParameters> service, IBaseService<GroupDto> baseService)
    {
        _service = service;
        _baseService = baseService;
    }

    // GET: Groups
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] GroupParameters parameters, CancellationToken token = default)
    {
        return Ok(await _service.GetByParametersAsync(parameters, token));
    }

    // GET: Groups/Details/5
    [HttpGet("{id:int?}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id, CancellationToken token = default)
    {
        return Ok(await _baseService.GetByIdAsync(id, token));
    }

    [HttpPost]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Post(GroupDto group, CancellationToken token = default)
    {
        await _baseService.CreateAsync(group, token);
        return Ok(group);
    }

    [HttpPut]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Put(GroupDto group, CancellationToken token = default)
    {
        await _baseService.UpdateAsync(group, token);
        return Ok(group);
    }

    [HttpDelete("{id:int?}")]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Delete(int id, CancellationToken token = default)
    {
        return Ok(await _baseService.DeleteAsync(id, token));
    }
}