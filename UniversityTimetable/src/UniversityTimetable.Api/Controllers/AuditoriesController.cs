using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Domain.Auth;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Shared.Interfaces.Domain;

namespace UniversityTimetable.Api.Controllers;

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
    public async Task<IActionResult> Get([FromQuery] AuditoryParameters parameters)
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
    public async Task<IActionResult> Post(AuditoryDto auditory)
    {
        await _baseService.CreateAsync(auditory);
        return Ok(auditory);
    }

    [HttpPut]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Put(AuditoryDto auditory)
    {
        await _baseService.UpdateAsync(auditory);
        return Ok(auditory);
    }

    [HttpDelete("{id:int?}")]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Delete(int? id)
    {
        return Ok(await _baseService.DeleteAsync(id));
    }
}