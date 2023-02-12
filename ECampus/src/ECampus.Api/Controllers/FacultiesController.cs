﻿using ECampus.Api.Metadata;
using ECampus.Contracts.Services;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Api.Controllers;

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