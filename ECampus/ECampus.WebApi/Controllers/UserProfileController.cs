using ECampus.Domain.DataTransferObjects;
using ECampus.Services.Contracts.Services;
using ECampus.WebApi.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.WebApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UserProfileController : ControllerBase
{
    private readonly IUserProfileService _profileService;
    private readonly IUserRelationsService _userRelationsService;

    public UserProfileController(IUserProfileService profileService, IUserRelationsService userRelationsService)
    {
        _profileService = profileService;
        _userRelationsService = userRelationsService;
    }

    [HttpGet("{id:int?}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id, CancellationToken token = default)
    {
        return Ok(await _profileService.GetByIdAsync(id, token));
    }
    
    [HttpPut]
    [Authorized]
    public async Task<IActionResult> Put(UserProfile user, CancellationToken token = default)
    {
        return Ok(await _profileService.UpdateProfileAsync(user, token));
    }

    [HttpPut("Validate/Create")]
    [AllowAnonymous]
    public async Task<IActionResult> ValidateCreate(UserDto user, CancellationToken token = default)
    {
        return Ok(await _profileService.ValidateCreateAsync(user, token));
    }

    [HttpPut("changePassword")]
    public async Task<IActionResult> ChangePassword([FromServices] IPasswordChangeService passwordChangeService,
        PasswordChangeDto passwordChange)
    {
        return Ok(await passwordChangeService.ChangePassword(passwordChange));
    }

    [HttpPut("changePassword/validate")]
    public async Task<IActionResult> ValidatePasswordChange([FromServices] IPasswordChangeService passwordChangeService,
        PasswordChangeDto passwordChange, CancellationToken token = default)
    {
        return Ok(await passwordChangeService.ValidatePasswordChange(passwordChange, token));
    }

    [HttpPut("Validate/Update")]
    [AllowAnonymous]
    public async Task<IActionResult> ValidateUpdate(UserDto user, CancellationToken token = default)
    {
        return Ok(await _profileService.ValidateUpdateAsync(user, token));
    }

    [HttpPost("auditory")]
    [Authorized]
    public async Task<IActionResult> SaveAuditory([FromQuery] int userId, [FromQuery] int auditoryId, CancellationToken token = default)
    {
        await _userRelationsService.SaveAuditory(userId, auditoryId, token);
        return NoContent();
    }

    [HttpDelete("auditory")]
    [Authorized]
    public async Task<IActionResult> RemoveAuditory([FromQuery] int userId, [FromQuery] int auditoryId, CancellationToken token = default)
    {
        await _userRelationsService.RemoveSavedAuditory(userId, auditoryId, token);
        return NoContent();
    }

    [HttpPost("group")]
    [Authorized]
    public async Task<IActionResult> SaveGroup([FromQuery] int userId, [FromQuery] int groupId, CancellationToken token = default)
    {
        await _userRelationsService.SaveGroup(userId, groupId, token);
        return NoContent();
    }

    [HttpDelete("group")]
    [Authorized]
    public async Task<IActionResult> RemoveGroup([FromQuery] int userId, [FromQuery] int groupId, CancellationToken token = default)
    {
        await _userRelationsService.RemoveSavedGroup(userId, groupId, token);
        return NoContent();
    }

    [HttpPost("teacher")]
    [Authorized]
    public async Task<IActionResult> SaveTeacher([FromQuery] int userId, [FromQuery] int teacherId, CancellationToken token = default)
    {
        await _userRelationsService.SaveTeacher(userId, teacherId, token);
        return NoContent();
    }

    [HttpDelete("teacher")]
    [Authorized]
    public async Task<IActionResult> RemoveTeacher([FromQuery] int userId, [FromQuery] int teacherId, CancellationToken token = default)
    {
        await _userRelationsService.RemoveSavedTeacher(userId, teacherId, token);
        return NoContent();
    }
}