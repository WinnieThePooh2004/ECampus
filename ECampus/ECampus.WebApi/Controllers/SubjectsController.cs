using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Domain.Requests.Subject;
using ECampus.Domain.Responses.Subject;
using ECampus.Services.Contracts.Services;
using ECampus.WebApi.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly IGetByParametersHandler<MultipleSubjectResponse, SubjectParameters> _handler;
    private readonly IBaseService<SubjectDto> _baseService;
        
    public SubjectsController(IGetByParametersHandler<MultipleSubjectResponse, SubjectParameters> handler,
        IBaseService<SubjectDto> baseService)
    {
        _handler = handler;
        _baseService = baseService;
    }

    // GET: Subjects
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] SubjectParameters parameters, CancellationToken token = default)
    {
        return Ok(await _handler.GetByParametersAsync(parameters, token));
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