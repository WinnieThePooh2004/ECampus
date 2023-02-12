using ECampus.Api.Metadata;
using ECampus.Contracts.Services;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimetableController : ControllerBase
{
    private readonly ITimetableService _service;
    private readonly IBaseService<ClassDto> _baseService;

    public TimetableController(ITimetableService service, IBaseService<ClassDto> baseService)
    {
        _service = service;
        _baseService = baseService;
    }

    [HttpGet("Auditory/{auditoryId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> AuditoryTimetable(int auditoryId, CancellationToken token = default)
    {
        var table = await _service.GetTimetableForAuditoryAsync(auditoryId, token);
        return Ok(table);
    }

    [HttpGet("Group/{groupId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GroupTimetable(int groupId, CancellationToken token = default)
    {
        var table = await _service.GetTimetableForGroupAsync(groupId, token);
        return Ok(table);
    }

    [HttpGet("Teacher/{teacherId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> TeacherTimetable(int teacherId, CancellationToken token = default)
    {
        var table = await _service.GetTimetableForTeacherAsync(teacherId, token);
        return Ok(table);
    }

    // GET: Classes/Details/5
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id, CancellationToken token = default)
    {
        return Ok(await _baseService.GetByIdAsync(id, token));
    }


    // GET: Classes/Create
    [HttpPost]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Post(ClassDto @class, CancellationToken token = default)
    {
        return Ok(await _baseService.CreateAsync(@class, token));
    }

    [HttpPut]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Put(ClassDto @class, CancellationToken token = default)
    {
        return Ok(await _baseService.UpdateAsync(@class, token));
    }

    [HttpDelete("{id:int?}")]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Delete(int id, CancellationToken token = default)
    {
        return Ok(await _baseService.DeleteAsync(id, token));
    }

    [HttpPut("Validate")]
    public async Task<IActionResult> Validate(ClassDto model, CancellationToken token = default)
    {
        return Ok(await _service.ValidateAsync(model, token));
    }
}