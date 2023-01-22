﻿using ECampus.Domain.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly IParametersService<SubjectDto, SubjectParameters> _service;
    private readonly IBaseService<SubjectDto> _baseService;
        
    public SubjectsController(IParametersService<SubjectDto, SubjectParameters> service, IBaseService<SubjectDto> baseService)
    {
        _service = service;
        _baseService = baseService;
    }

    // GET: Subjects
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] SubjectParameters parameters)
    {
        return Ok(await _service.GetByParametersAsync(parameters));
    }

    // GET: Subjects/Details/5
    [HttpGet("{id:int?}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int? id)
    {
        return Ok(await _baseService.GetByIdAsync(id));
    }

    [HttpPost]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Post(SubjectDto subject)
    {
        await _baseService.CreateAsync(subject);
        return Ok(subject);
    }

    [HttpPut]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Put(SubjectDto subject)
    {
        await _baseService.UpdateAsync(subject);
        return Ok(subject);
    }

    [HttpDelete("{id:int?}")]
    [Authorized(UserRole.Admin)]
    public async Task<IActionResult> Delete(int? id)
    {
        return Ok(await _baseService.DeleteAsync(id));
    }
}