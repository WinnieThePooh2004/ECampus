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
        return Ok(await _service.ValidateCreateAsync(user, HttpContext));
    }

    [HttpPut("/changePassword")]
    public async Task<IActionResult> ChangePassword(PasswordChangeDto passwordChange)
    {
        return Ok(await _service.ChangePassword(passwordChange));
    }

    [HttpPut("Validate/Update")]
    [AllowAnonymous]
    public async Task<IActionResult> ValidateUpdate(UserDto user)
    {
        return Ok(await _service.ValidateUpdateAsync(user));
    }

    [HttpPost("auditory/{auditoryId:int}")]
    [Authorized]
    public async Task<IActionResult> SaveAuditory(int auditoryId)
    {
        await _service.SaveAuditory(HttpContext.User, auditoryId);
        return NoContent();
    }
        
    [HttpDelete("auditory/{auditoryId:int}")]
    [Authorized]
    public async Task<IActionResult> RemoveAuditory(int auditoryId)
    {
        await _service.RemoveSavedAuditory(HttpContext.User, auditoryId);
        return NoContent();
    }

    [HttpPost("group/{groupId:int}")]
    [Authorized]
    public async Task<IActionResult> SaveGroup(int groupId)
    {
        await _service.SaveGroup(HttpContext.User, groupId);
        return NoContent();
    }
    
    [HttpDelete("group/{groupId:int}")]
    [Authorized]
    public async Task<IActionResult> RemoveGroup(int groupId)
    {
        await _service.RemoveSavedGroup(HttpContext.User, groupId);
        return NoContent();
    }

    [HttpPost("teacher/{teacherId:int}")]
    [Authorized]
    public async Task<IActionResult> SaveTeacher(int teacherId)
    {
        await _service.SaveTeacher(HttpContext.User, teacherId);
        return NoContent();
    }
        
    [HttpDelete("teacher/{teacherId:int}")]
    [Authorized]
    public async Task<IActionResult> RemoveTeacher(int teacherId)
    {
        await _service.RemoveSavedTeacher(HttpContext.User, teacherId);
        return NoContent();
    }
}