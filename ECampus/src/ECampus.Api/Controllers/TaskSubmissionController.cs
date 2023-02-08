using ECampus.Api.Metadata;
using ECampus.Contracts.Services;
using ECampus.Services.Services.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
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
    public async Task<IActionResult> Get([FromQuery] TaskSubmissionParameters parameters, CancellationToken token = default)
    {
        return Ok(await _parametersService.GetByParametersAsync(parameters));
    }

    [HttpGet("{id:int}")]
    [Authorized(UserRole.Student, UserRole.Teacher, UserRole.Admin)]
    public async Task<IActionResult> Get(int id, CancellationToken token = default)
    {
        return Ok(await _taskSubmissionService.GetByIdAsync(id, token));
    }

    [HttpGet("byCourseTask/{courseTaskId:int}")]
    [Authorized(UserRole.Student)]
    public async Task<IActionResult> GetByCourse(int courseTaskId, CancellationToken token = default)
    {
        return Ok(await _taskSubmissionService.GetByCourseAsync(courseTaskId, token));
    }

    [HttpPut("content/{taskSubmissionId:int}")]
    [Authorized(UserRole.Student)]
    public async Task<IActionResult> UpdateContent([FromRoute] int taskSubmissionId, [FromBody] string content, CancellationToken token = default)
    {
        return Ok(await _taskSubmissionService.UpdateContentAsync(taskSubmissionId, content, token));
    }
    
    [HttpPut("mark/{taskSubmissionId:int}")]
    [Authorized(UserRole.Teacher)]
    public async Task<IActionResult> UpdateMark([FromRoute] int taskSubmissionId, [FromBody] int mark, CancellationToken token = default)
    {
        return Ok(await _taskSubmissionService.UpdateMarkAsync(taskSubmissionId, mark, token));
    }
}