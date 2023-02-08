using ECampus.Api.Metadata;
using ECampus.Contracts.Services;
using ECampus.Services.Services.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly IParametersService<TeacherDto, TeacherParameters> _service;
    private readonly IBaseService<TeacherDto> _baseService;

    public TeachersController(IParametersService<TeacherDto, TeacherParameters> service,
        IBaseService<TeacherDto> baseService)
    {
        _service = service;
        _baseService = baseService;
    }

    // GET: Teachers
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] TeacherParameters parameters, CancellationToken token = default)
    {
        return Ok(await _service.GetByParametersAsync(parameters));
    }

    // GET: Teachers/Details/5
    [HttpGet("{id:int?}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id, CancellationToken token = default)
    {
        return Ok(await _baseService.GetByIdAsync(id, token));
    }

    [HttpPost]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Post(TeacherDto teacher, CancellationToken token = default)
    {
        await _baseService.CreateAsync(teacher, token);
        return Ok(teacher);
    }

    [HttpPut]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Put(TeacherDto teacher, CancellationToken token = default)
    {
        await _baseService.UpdateAsync(teacher, token);
        return Ok(teacher);
    }

    [HttpDelete("{id:int}")]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Delete(int id, CancellationToken token = default)
    {
        return Ok(await _baseService.DeleteAsync(id, token));
    }
}