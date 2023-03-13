using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Domain.Requests.Faculty;
using ECampus.Domain.Responses.Faculty;
using ECampus.Services.Contracts.Services;
using ECampus.WebApi.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacultiesController : ControllerBase
{
    private readonly IParametersService<MultipleFacultyResponse, FacultyParameters> _service;
    private readonly IBaseService<FacultyDto> _baseService;
    public FacultiesController(IParametersService<MultipleFacultyResponse, FacultyParameters> service,
        IBaseService<FacultyDto> baseService)
    {
        _service = service;
        _baseService = baseService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] FacultyParameters parameters, CancellationToken token = default)
    {
        return Ok(await _service.GetByParametersAsync(parameters, token));
    }

    [HttpGet("{id:int?}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _baseService.GetByIdAsync(id));
    }

    [HttpPost]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Post([Bind("Name")] FacultyDto faculty, CancellationToken token = default)
    {
        await _baseService.CreateAsync(faculty, token);
        return Ok(faculty);
    }

    [HttpPut]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Put(FacultyDto faculty, CancellationToken token = default)
    {
        await _baseService.UpdateAsync(faculty, token);
        return Ok(faculty);
    }

    [HttpDelete("{id:int?}")]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Delete(int id, CancellationToken token = default)
    {
        return Ok(await _baseService.DeleteAsync(id, token));
    }
}