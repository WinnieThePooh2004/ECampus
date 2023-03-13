using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Domain.Requests.Teacher;
using ECampus.Domain.Responses.Teacher;
using ECampus.Services.Contracts.Services;
using ECampus.WebApi.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly IGetByParametersHandler<MultipleTeacherResponse, TeacherParameters> _handler;
    private readonly IBaseService<TeacherDto> _baseService;

    public TeachersController(IGetByParametersHandler<MultipleTeacherResponse, TeacherParameters> handler,
        IBaseService<TeacherDto> baseService)
    {
        _handler = handler;
        _baseService = baseService;
    }

    // GET: Teachers
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] TeacherParameters parameters, CancellationToken token = default)
    {
        return Ok(await _handler.GetByParametersAsync(parameters, token));
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