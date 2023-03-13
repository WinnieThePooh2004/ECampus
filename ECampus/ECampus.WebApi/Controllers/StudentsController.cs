using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Requests.Student;
using ECampus.Domain.Responses.Student;
using ECampus.Services.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IGetByParametersHandler<MultipleStudentResponse, StudentParameters> _getByParametersHandler;
    private readonly IBaseService<StudentDto> _baseService;

    public StudentsController(IGetByParametersHandler<MultipleStudentResponse, StudentParameters> getByParametersHandler,
        IBaseService<StudentDto> baseService)
    {
        _getByParametersHandler = getByParametersHandler;
        _baseService = baseService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] StudentParameters parameters, CancellationToken token = default)
    {
        return Ok(await _getByParametersHandler.GetByParametersAsync(parameters, token));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken token = default)
    {
        return Ok(await _baseService.GetByIdAsync(id, token));
    }

    [HttpPost]
    public async Task<IActionResult> Post(StudentDto student, CancellationToken token = default)
    {
        return Ok(await _baseService.CreateAsync(student, token));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken token = default)
    {
        return Ok(await _baseService.DeleteAsync(id, token));
    }

    [HttpPut]
    public async Task<IActionResult> Put(StudentDto student, CancellationToken token = default)
    {
        return Ok(await _baseService.UpdateAsync(student, token));
    }
}