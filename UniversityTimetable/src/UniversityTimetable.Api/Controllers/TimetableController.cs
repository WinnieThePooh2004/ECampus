using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Domain.Auth;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Domain;

namespace UniversityTimetable.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimetableController : ControllerBase
{
    private readonly IClassService _service;
    private readonly IBaseService<ClassDto> _baseService;

    public TimetableController(IClassService service, IBaseService<ClassDto> baseService)
    {
        _service = service;
        _baseService = baseService;
    }

    [HttpGet("Auditory/{auditoryId:int?}")]
    [AllowAnonymous]
    public async Task<IActionResult> AuditoryTimetable(int? auditoryId)
    {
        var table = await _service.GetTimetableForAuditoryAsync(auditoryId);
        return Ok(table);
    }

    [HttpGet("Group/{groupId:int?}")]
    [AllowAnonymous]
    public async Task<IActionResult> GroupTimetable(int? groupId)
    {
        var table = await _service.GetTimetableForGroupAsync(groupId);
        return Ok(table);
    }

    [HttpGet("Teacher/{teacherId:int?}")]
    [AllowAnonymous]
    public async Task<IActionResult> TeacherTimetable(int? teacherId)
    {
        var table = await _service.GetTimetableForTeacherAsync(teacherId);
        return Ok(table);
    }

    // GET: Classes/Details/5
    [HttpGet("{id:int?}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int? id)
    {
        return Ok(await _baseService.GetByIdAsync(id));
    }


    // GET: Classes/Create
    [HttpPost]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Post(ClassDto @class)
    {
        return Ok(await _baseService.CreateAsync(@class));
    }

    [HttpPut]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Put(ClassDto @class)
    {
        return Ok(await _baseService.UpdateAsync(@class));
    }

    [HttpDelete("{id:int?}")]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Delete(int? id)
    {
        return Ok(await _baseService.DeleteAsync(id));
    }

    [HttpPut("Validate")]
    public async Task<IActionResult> Validate(ClassDto model)
    {
        return Ok(await _service.ValidateAsync(model));
    }
}