using ECampus.Services.Contracts.Services;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.QueryParameters;
using ECampus.WebApi.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorized(UserRole.Admin)]
public class LogsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromServices] IParametersService<LogDto, LogParameters> service,
        [FromQuery] LogParameters parameters)
    {
        return Ok(await service.GetByParametersAsync(parameters));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromServices] IBaseService<LogDto> service, int id)
    {
        return Ok(await service.DeleteAsync(id));
    }
}