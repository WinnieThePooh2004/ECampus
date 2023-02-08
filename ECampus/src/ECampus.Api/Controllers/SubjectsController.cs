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
public class SubjectsController : ControllerBase
{
    private readonly IParametersService<SubjectDto, SubjectParameters> _service;
    private readonly IBaseService<SubjectDto> _baseService;
        
    public SubjectsController(IParametersService<SubjectDto, SubjectParameters> service, IBaseService<SubjectDto> baseService)
    {
        _service = service;
        _baseService = baseService;
    }

    // GET: Subjects
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] SubjectParameters parameters, CancellationToken token = default)
    {
        return Ok(await _service.GetByParametersAsync(parameters, token));
    }

    // GET: Subjects/Details/5
    [HttpGet("{id:int?}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id, CancellationToken token = default)
    {
        return Ok(await _baseService.GetByIdAsync(id, token));
    }

    [HttpPost]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Post(SubjectDto subject, CancellationToken token = default)
    {
        await _baseService.CreateAsync(subject, token);
        return Ok(subject);
    }

    [HttpPut]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Put(SubjectDto subject, CancellationToken token = default)
    {
        await _baseService.UpdateAsync(subject, token);
        return Ok(subject);
    }

    [HttpDelete("{id:int?}")]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Delete(int id, CancellationToken token = default)
    {
        return Ok(await _baseService.DeleteAsync(id, token));
    }
}