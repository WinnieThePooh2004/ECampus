using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Domain.Requests.CourseTask;
using ECampus.Domain.Responses.CourseTask;
using ECampus.Services.Contracts.Services;
using ECampus.WebApi.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseTasksController : ControllerBase
{
    private readonly IParametersService<MultipleCourseTaskResponse, CourseTaskParameters> _parametersService;
    private readonly IBaseService<CourseTaskDto> _baseService;

    public CourseTasksController(IParametersService<MultipleCourseTaskResponse, CourseTaskParameters> parametersService,
        IBaseService<CourseTaskDto> baseService)
    {
        _parametersService = parametersService;
        _baseService = baseService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] CourseTaskParameters parameters, CancellationToken token = default)
    {
        return Ok(await _parametersService.GetByParametersAsync(parameters, token));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id, CancellationToken token = default)
    {
        return Ok(await _baseService.GetByIdAsync(id, token));
    }

    [HttpPost]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Post(CourseTaskDto task, CancellationToken token = default)
    {
        await _baseService.CreateAsync(task, token);
        return Ok(task);
    }

    [HttpPut]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Put(CourseTaskDto task, CancellationToken token = default)
    {
        await _baseService.UpdateAsync(task, token);
        return Ok(task);
    }

    [HttpDelete("{id:int?}")]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Delete(int id, CancellationToken token = default)
    {
        return Ok(await _baseService.DeleteAsync(id, token));
    }
}