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
    private readonly IBaseService<TaskSubmissionDto> _baseService;
    private readonly IParametersService<TaskSubmissionDto, TaskSubmissionParameters> _parametersService;

    public TaskSubmissionsController(IBaseService<TaskSubmissionDto> baseService,
        IParametersService<TaskSubmissionDto, TaskSubmissionParameters> parametersService)
    {
        _baseService = baseService;
        _parametersService = parametersService;
    }

    [HttpGet]
    [Authorized(UserRole.Student)]
    public async Task<IActionResult> Get([FromQuery] TaskSubmissionParameters parameters)
    {
        return Ok(await _parametersService.GetByParametersAsync(parameters));
    }

    [HttpGet("{id:int}")]
    [Authorized(UserRole.Student)]
    public async Task<IActionResult> Get(int? id)
    {
        return Ok(await _baseService.GetByIdAsync(id));
    }

    [HttpPost]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Post(TaskSubmissionDto course)
    {
        await _baseService.CreateAsync(course);
        return Ok(course);
    }

    [HttpPut]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Put(TaskSubmissionDto course)
    {
        await _baseService.UpdateAsync(course);
        return Ok(course);
    }

    [HttpDelete("{id:int?}")]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Delete(int? id)
    {
        return Ok(await _baseService.DeleteAsync(id));
    }
}