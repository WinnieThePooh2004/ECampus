using ECampus.Domain.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    private readonly IBaseService<UserDto> _baseService;
    private readonly IParametersService<UserDto, UserParameters> _parametersService;
    private readonly IUserRelationsService _userRelationsService;

    public UsersController(IUserService service, IBaseService<UserDto> baseService,
        IParametersService<UserDto, UserParameters> parametersService, IUserRelationsService userRelationsService)
    {
        _service = service;
        _baseService = baseService;
        _parametersService = parametersService;
        _userRelationsService = userRelationsService;
    }

    [HttpGet("{id:int?}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int? id)
    {
        return Ok(await _baseService.GetByIdAsync(id));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] UserParameters parameters)
    {
        return Ok(await _parametersService.GetByParametersAsync(parameters));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Post(UserDto user)
    {
        return Ok(await _baseService.CreateAsync(user));
    }

    [HttpPut]
    [Authorized]
    public async Task<IActionResult> Put(UserDto user)
    {
        return Ok(await _baseService.UpdateAsync(user));
    }

    [HttpDelete("{id:int?}")]
    [Authorized]
    public async Task<IActionResult> Delete(int? id)
    {
        return Ok(await _baseService.DeleteAsync(id));
    }

    [HttpPut("Validate/Create")]
    [AllowAnonymous]
    public async Task<IActionResult> ValidateCreate(UserDto user)
    {
        return Ok(await _service.ValidateCreateAsync(user));
    }

    [HttpPut("changePassword")]
    public async Task<IActionResult> ChangePassword(PasswordChangeDto passwordChange)
    {
        return Ok(await _service.ChangePassword(passwordChange));
    }

    [HttpPut("changePassword/validate")]
    public async Task<IActionResult> ValidatePasswordChange(PasswordChangeDto passwordChange)
    {
        return Ok(await _service.ValidatePasswordChange(passwordChange));
    }

    [HttpPut("Validate/Update")]
    [AllowAnonymous]
    public async Task<IActionResult> ValidateUpdate(UserDto user)
    {
        return Ok(await _service.ValidateUpdateAsync(user));
    }

    [HttpPost("auditory")]
    [Authorized]
    public async Task<IActionResult> SaveAuditory([FromQuery] int userId, [FromQuery] int auditoryId)
    {
        await _userRelationsService.SaveAuditory(userId, auditoryId);
        return NoContent();
    }

    [HttpDelete("auditory")]
    [Authorized]
    public async Task<IActionResult> RemoveAuditory([FromQuery] int userId, [FromQuery] int auditoryId)
    {
        await _userRelationsService.RemoveSavedAuditory(userId, auditoryId);
        return NoContent();
    }

    [HttpPost("group")]
    [Authorized]
    public async Task<IActionResult> SaveGroup([FromQuery] int userId, [FromQuery] int groupId)
    {
        await _userRelationsService.SaveGroup(userId, groupId);
        return NoContent();
    }

    [HttpDelete("group")]
    [Authorized]
    public async Task<IActionResult> RemoveGroup([FromQuery] int userId, [FromQuery] int groupId)
    {
        await _userRelationsService.RemoveSavedGroup(userId, groupId);
        return NoContent();
    }

    [HttpPost("teacher")]
    [Authorized]
    public async Task<IActionResult> SaveTeacher([FromQuery] int userId, [FromQuery] int teacherId)
    {
        await _userRelationsService.SaveTeacher(userId, teacherId);
        return NoContent();
    }

    [HttpDelete("teacher")]
    [Authorized]
    public async Task<IActionResult> RemoveTeacher([FromQuery] int userId, [FromQuery] int teacherId)
    {
        await _userRelationsService.RemoveSavedTeacher(userId, teacherId);
        return NoContent();
    }
}