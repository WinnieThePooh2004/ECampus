using ECampus.Contracts.Services;
using ECampus.Domain.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorized(UserRole.Admin)]
public class UserRolesController : ControllerBase
{
    private readonly IUserRolesService _service;

    public UserRolesController(IUserRolesService service)
    {
        _service = service;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    [HttpPut]
    public async Task<IActionResult> Put(UserDto user)
    {
        return Ok(await _service.UpdateAsync(user));
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(UserDto user)
    {
        return Ok(await _service.CreateAsync(user));
    }
}