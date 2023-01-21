using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Domain.Auth;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly IParametersService<CourseDto, CourseParameters> _parametersService;
    private readonly IBaseService<CourseDto> _baseService;

    public CoursesController(IParametersService<CourseDto, CourseParameters> parametersService,
        IBaseService<CourseDto> baseService)
    {
        _parametersService = parametersService;
        _baseService = baseService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] CourseParameters parameters)
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
    public async Task<IActionResult> Post(CourseDto course)
    {
        await _baseService.CreateAsync(course);
        return Ok(course);
    }

    [HttpPut]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Put(CourseDto course)
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