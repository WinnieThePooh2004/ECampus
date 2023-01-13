using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IParametersService<StudentDto, StudentParameters> _parametersService;
    private readonly IBaseService<StudentDto> _baseService;

    public StudentsController(IParametersService<StudentDto, StudentParameters> parametersService, IBaseService<StudentDto> baseService)
    {
        _parametersService = parametersService;
        _baseService = baseService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] StudentParameters parameters)
    {
        return Ok(await _parametersService.GetByParametersAsync(parameters));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _baseService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Post(StudentDto student)
    {
        return Ok(await _baseService.UpdateAsync(student));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        return Ok(await _baseService.DeleteAsync(id));
    }

    [HttpPut]
    public async Task<IActionResult> Put(StudentDto student)
    {
        return Ok(await _baseService.UpdateAsync(student));
    }
}