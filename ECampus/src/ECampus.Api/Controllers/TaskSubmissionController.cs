﻿using ECampus.Api.Metadata;
using ECampus.Contracts.Services;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Mvc;

namespace ECampus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskSubmissionsController : ControllerBase
{
    private readonly IParametersService<TaskSubmissionDto, TaskSubmissionParameters> _parametersService;
    private readonly ITaskSubmissionService _taskSubmissionService;

    public TaskSubmissionsController(IParametersService<TaskSubmissionDto, TaskSubmissionParameters> parametersService,
        ITaskSubmissionService taskSubmissionService)
    {
        _parametersService = parametersService;
        _taskSubmissionService = taskSubmissionService;
    }

    [HttpGet]
    [Authorized(UserRole.Teacher)]
    public async Task<IActionResult> Get([FromQuery] TaskSubmissionParameters parameters, CancellationToken token = default)
    {
        return Ok(await _parametersService.GetByParametersAsync(parameters, token));
    }

    [HttpGet("{id:int}")]
    [Authorized(UserRole.Student, UserRole.Teacher, UserRole.Admin)]
    public async Task<IActionResult> Get(int id, CancellationToken token = default)
    {
        return Ok(await _taskSubmissionService.GetByIdAsync(id, token));
    }

    [HttpGet("byCourseTask/{courseTaskId:int}")]
    [Authorized(UserRole.Student)]
    public async Task<IActionResult> GetByCourse(int courseTaskId, CancellationToken token = default)
    {
        return Ok(await _taskSubmissionService.GetByCourseAsync(courseTaskId, token));
    }

    [HttpPut("content")]
    [Authorized(UserRole.Student)]
    public async Task<IActionResult> UpdateContent(UpdateSubmissionContentDto dto, CancellationToken token = default)
    {
        return Ok(await _taskSubmissionService.UpdateContentAsync(dto.SubmissionId, dto.Content, token));
    }
    
    [HttpPut("mark")]
    [Authorized(UserRole.Teacher)]
    public async Task<IActionResult> UpdateMark(UpdateSubmissionMarkDto dto, CancellationToken token = default)
    {
        return Ok(await _taskSubmissionService.UpdateMarkAsync(dto.SubmissionId, dto.Mark, token));
    }
}