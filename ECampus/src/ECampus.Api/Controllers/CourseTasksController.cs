using ECampus.Contracts.Services;
using ECampus.Domain.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseTasksController : ControllerBase
{
    private readonly IParametersService<CourseTaskDto, CourseTaskParameters> _parametersService;
    private readonly IBaseService<CourseTaskDto> _baseService;

    public CourseTasksController(IParametersService<CourseTaskDto, CourseTaskParameters> parametersService,
        IBaseService<CourseTaskDto> baseService)
    {
        _parametersService = parametersService;
        _baseService = baseService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] CourseTaskParameters parameters)
    {
        return Ok(await _parametersService.GetByParametersAsync(parameters));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int? id)
    {
        return Ok(await _baseService.GetByIdAsync(id));
    }

    [HttpPost]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Post(CourseTaskDto task)
    {
        await _baseService.CreateAsync(task);
        return Ok(task);
    }

    [HttpPut]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Put(CourseTaskDto task)
    {
        await _baseService.UpdateAsync(task);
        return Ok(task);
    }

    [HttpDelete("{id:int?}")]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Delete(int? id)
    {
        return Ok(await _baseService.DeleteAsync(id));
    }
}