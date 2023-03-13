using ECampus.Domain.Commands.Auditory;
using ECampus.Domain.Entities;
using ECampus.Domain.Enums;
using ECampus.Domain.Requests.Auditory;
using ECampus.Domain.Responses.Auditory;
using ECampus.Services.Contracts;
using ECampus.WebApi.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditoriesController : ControllerBase
{
    private readonly IDomainAccessFacade _domainAccess;
        
    public AuditoriesController(IDomainAccessFacade domainAccess)
    {
        _domainAccess = domainAccess;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] AuditoryParameters parameters, CancellationToken token = default)
    {
        return Ok(await _domainAccess.GetByParametersAsync<MultipleAuditoryResponse, AuditoryParameters>(parameters, token));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id, CancellationToken token = default)
    {
        return Ok(await _domainAccess.GetByIdAsync<SingleAuditoryResponse>(id, token));
    }

    [HttpPost]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Post(CreateAuditoryCommand auditory, CancellationToken token = default)
    {
        var result = await _domainAccess.CreateAsync(auditory, token);
        return Ok(result);
    }

    [HttpPut]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Put(UpdateAuditoryCommand auditory, CancellationToken token = default)
    {
        await _domainAccess.UpdateAsync(auditory, token);
        return NoContent();
    }

    [HttpDelete("{id:int?}")]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Delete(int id, CancellationToken token = default)
    {
        await _domainAccess.DeleteAsync<Auditory>(id, token);
        return NoContent();
    }
}