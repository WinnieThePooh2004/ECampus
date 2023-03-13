using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Domain.Requests.User;
using ECampus.Domain.Responses.User;
using ECampus.Services.Contracts.Services;
using ECampus.WebApi.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IBaseService<UserDto> _service;
    private readonly IGetByParametersHandler<MultipleUserResponse, UserParameters> _getByParametersHandler;

    public UsersController(IBaseService<UserDto> service,
        IGetByParametersHandler<MultipleUserResponse, UserParameters> getByParametersHandler)
    {
        _service = service;
        _getByParametersHandler = getByParametersHandler;
    }
    
    [HttpGet("{id:int}")]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Get(int id, CancellationToken token = default)
    {
        return Ok(await _service.GetByIdAsync(id, token));
    }

    [HttpGet]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Get([FromQuery] UserParameters parameters, CancellationToken token = default)
    {
        return Ok(await _getByParametersHandler.GetByParametersAsync(parameters, token));
    }

    [HttpPut]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Put(UserDto user, CancellationToken token = default)
    {
        return Ok(await _service.UpdateAsync(user, token));
    }

    [HttpPost]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Post(UserDto user, CancellationToken token = default)
    {
        return Ok(await _service.CreateAsync(user, token));
    }

    [HttpDelete("{id:int}")]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        return Ok(await _service.DeleteAsync(id));
    }
}