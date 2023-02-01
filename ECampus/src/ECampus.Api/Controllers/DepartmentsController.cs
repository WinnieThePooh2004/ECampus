using ECampus.Contracts.Services;
using ECampus.Domain.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IParametersService<DepartmentDto, DepartmentParameters> _parametersService;
    private readonly IBaseService<DepartmentDto> _baseService;

    public DepartmentsController(IParametersService<DepartmentDto, DepartmentParameters> parametersService,
        IBaseService<DepartmentDto> baseService)
    {
        _parametersService = parametersService;
        _baseService = baseService;
    }

    // GET: Departments
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] DepartmentParameters parameters)
    {
        return Ok(await _parametersService.GetByParametersAsync(parameters));
    }

    // GET: Departments/Details/5
    [HttpGet("{id:int?}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int? id)
    {
        return Ok(await _baseService.GetByIdAsync(id));
    }

    // POST: Departments/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Post(DepartmentDto department)
    {
        await _baseService.CreateAsync(department);
        return Ok(department);
    }

    [HttpPut]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Put(DepartmentDto department)
    {
        await _baseService.UpdateAsync(department);
        return Ok(department);
    }

    // GET: Departments/Delete/5
    [HttpDelete("{id:int?}")]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Delete(int? id)
    {
        return Ok(await _baseService.DeleteAsync(id));
    }
}