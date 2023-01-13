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
public class FacultiesController : ControllerBase
{
    private readonly IParametersService<FacultyDto, FacultyParameters> _service;
    private readonly IBaseService<FacultyDto> _baseService;
    public FacultiesController(IParametersService<FacultyDto, FacultyParameters> service, IBaseService<FacultyDto> baseService)
    {
        _service = service;
        _baseService = baseService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] FacultyParameters parameters)
    {
        return Ok(await _service.GetByParametersAsync(parameters));
    }

    [HttpGet("{id:int?}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int? id)
    {
        return Ok(await _baseService.GetByIdAsync(id));
    }

    [HttpPost]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Post([Bind("Name")] FacultyDto faculty)
    {
        await _baseService.CreateAsync(faculty);
        return Ok(faculty);
    }

    [HttpPut]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Put(FacultyDto faculty)
    {
        await _baseService.UpdateAsync(faculty);
        return Ok(faculty);
    }

    [HttpDelete("{id:int?}")]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Delete(int? id)
    {
        return Ok(await _baseService.DeleteAsync(id));
    }
}