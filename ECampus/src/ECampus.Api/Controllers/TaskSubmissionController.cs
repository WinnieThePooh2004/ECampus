using ECampus.Domain.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskSubmissionsController : ControllerBase
{
    private readonly IParametersService<TaskSubmissionDto, TaskSubmissionParameters> _parametersService;
    private readonly ITaskSubmissionService _taskSubmissionService;

    public TaskSubmissionsController(IParametersService<TaskSubmissionDto, TaskSubmissionParameters> parametersService,
        ITaskSubmissionService taskSubmissionService)
    {
        _parametersService = parametersService;
        _taskSubmissionService = taskSubmissionService;
    }

    [HttpGet]
    [Authorized(UserRole.Teacher)]
    public async Task<IActionResult> Get([FromQuery] TaskSubmissionParameters parameters)
    {
        return Ok(await _parametersService.GetByParametersAsync(parameters));
    }

    [HttpGet("{id:int}")]
    [Authorized(UserRole.Teacher, UserRole.Admin)]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _taskSubmissionService.GetByIdAsync(id));
    }

    [HttpGet("byCourseTask/{courseTaskId:int}")]
    [Authorized(UserRole.Student)]
    public async Task<IActionResult> GetByCourse(int courseTaskId)
    {
        return Ok(await _taskSubmissionService.GetByCourse(courseTaskId));
    }

    [HttpPut("content/{taskSubmissionId:int}")]
    [Authorized(UserRole.Student)]
    public async Task<IActionResult> UpdateContent([FromRoute] int taskSubmissionId, [FromBody] string content)
    {
        await _taskSubmissionService.UpdateContent(taskSubmissionId, content);
        return NoContent();
    }
    
    [HttpPut("mark/{taskSubmissionId:int}")]
    [Authorized(UserRole.Teacher)]
    public async Task<IActionResult> UpdateMark([FromRoute] int taskSubmissionId, [FromBody] int mark)
    {
        await _taskSubmissionService.UpdateMark(taskSubmissionId, mark);
        return NoContent();
    }
}