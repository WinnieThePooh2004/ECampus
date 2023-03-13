using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Domain.Requests.Course;
using ECampus.Domain.Responses.Course;
using ECampus.Services.Contracts.Services;
using ECampus.WebApi.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly IGetByParametersHandler<MultipleCourseResponse, CourseParameters> _getByParametersHandler;
    private readonly IBaseService<CourseDto> _baseService;

    public CoursesController(IGetByParametersHandler<MultipleCourseResponse, CourseParameters> getByParametersHandler,
        IBaseService<CourseDto> baseService)
    {
        _getByParametersHandler = getByParametersHandler;
        _baseService = baseService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] CourseParameters parameters, CancellationToken token = default)
    {
        return Ok(await _getByParametersHandler.GetByParametersAsync(parameters, token));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id, CancellationToken token = default)
    {
        return Ok(await _baseService.GetByIdAsync(id, token));
    }

    [HttpGet("summary")]
    [Authorized(UserRole.Student)]
    public async Task<IActionResult> Summary(
        [FromServices] IGetByParametersHandler<CourseSummaryResponse, CourseSummaryParameters> getByParametersHandler,
        [FromQuery] CourseSummaryParameters parameters)
    {
        return Ok(await getByParametersHandler.GetByParametersAsync(parameters));
    }

    [HttpPost]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Post(CourseDto course, CancellationToken token = default)
    {
        await _baseService.CreateAsync(course, token);
        return Ok(course);
    }

    [HttpPut]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Put(CourseDto course, CancellationToken token = default)
    {
        await _baseService.UpdateAsync(course, token);
        return Ok(course);
    }

    [HttpDelete("{id:int?}")]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Delete(int id, CancellationToken token = default)
    {
        return Ok(await _baseService.DeleteAsync(id, token));
    }
}