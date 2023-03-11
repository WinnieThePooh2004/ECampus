using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Domain.QueryParameters;
using ECampus.Services.Contracts.Services;
using ECampus.WebApi.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditoriesController : ControllerBase
{
    private readonly IParametersService<AuditoryDto, AuditoryParameters> _parametersService;
    private readonly IBaseService<AuditoryDto> _baseService;

    public AuditoriesController(IParametersService<AuditoryDto, AuditoryParameters> parametersService,
        IBaseService<AuditoryDto> baseService)
    {
        _parametersService = parametersService;
        _baseService = baseService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] AuditoryParameters parameters, CancellationToken token = default)
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
    public async Task<IActionResult> Post(AuditoryDto auditory, CancellationToken token = default)
    {
        await _baseService.CreateAsync(auditory, token);
        return Ok(auditory);
    }

    [HttpPut]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Put(AuditoryDto auditory, CancellationToken token = default)
    {
        await _baseService.UpdateAsync(auditory, token);
        return Ok(auditory);
    }

    [HttpDelete("{id:int?}")]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Delete(int id, CancellationToken token = default)
    {
        return Ok(await _baseService.DeleteAsync(id, token));
    }
}