using ECampus.Contracts.Services;
using ECampus.Domain.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IParametersService<GroupDto, GroupParameters> _service;
    private readonly IBaseService<GroupDto> _baseService;

    public GroupsController(IParametersService<GroupDto, GroupParameters> service, IBaseService<GroupDto> baseService)
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