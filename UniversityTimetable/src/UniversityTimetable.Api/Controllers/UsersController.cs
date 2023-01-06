using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Domain.Auth;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Domain;

namespace UniversityTimetable.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpGet("{id:int?}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int? id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Post(UserDto user)
    {
        return Ok(await _service.CreateAsync(user));
    }

    [HttpPut]
    [Authorized]
    public async Task<IActionResult> Put(UserDto user)
    {
        return Ok(await _service.UpdateAsync(user));
    }

    [HttpDelete("{id:int?}")]
    [Authorized]
    public async Task<IActionResult> Delete(int? id)
    {
        await _service.DeleteAsync(id);
        return Ok(id);
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
        await _service.SaveAuditory(userId, auditoryId);
        return NoContent();
    }
        
    [HttpDelete("auditory")]
    [Authorized]
    public async Task<IActionResult> RemoveAuditory([FromQuery] int userId, [FromQuery] int auditoryId)
    {
        await _service.RemoveSavedAuditory(userId, auditoryId);
        return NoContent();
    }

    [HttpPost("group")]
    [Authorized]
    public async Task<IActionResult> SaveGroup([FromQuery] int userId, [FromQuery] int groupId)
    {
        await _service.SaveGroup(userId, groupId);
        return NoContent();
    }
    
    [HttpDelete("group")]
    [Authorized]
    public async Task<IActionResult> RemoveGroup([FromQuery] int userId, [FromQuery] int groupId)
    {
        await _service.RemoveSavedGroup(userId, groupId);
        return NoContent();
    }

    [HttpPost("teacher")]
    [Authorized]
    public async Task<IActionResult> SaveTeacher([FromQuery] int userId, [FromQuery] int teacherId)
    {
        await _service.SaveTeacher(userId, teacherId);
        return NoContent();
    }
    
    [HttpDelete("teacher")]
    [Authorized]
    public async Task<IActionResult> RemoveTeacher([FromQuery] int userId, [FromQuery] int teacherId)
    {
        await _service.RemoveSavedTeacher(userId, teacherId);
        return NoContent();
    }
}