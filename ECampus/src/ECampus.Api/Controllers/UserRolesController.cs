using ECampus.Api.Metadata;
using ECampus.Contracts.Services;
using ECampus.Services.Services.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorized(UserRole.Admin)]
public class UserRolesController : ControllerBase
{
    private readonly IBaseService<UserDto> _service;

    public UserRolesController(IBaseService<UserDto> service)
    {
        _service = service;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken token = default)
    {
        return Ok(await _service.GetByIdAsync(id, token));
    }

    [HttpPut]
    public async Task<IActionResult> Put(UserDto user, CancellationToken token = default)
    {
        return Ok(await _service.UpdateAsync(user, token));
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(UserDto user, CancellationToken token = default)
    {
        return Ok(await _service.CreateAsync(user, token));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        return Ok(await _service.DeleteAsync(id));
    }
}