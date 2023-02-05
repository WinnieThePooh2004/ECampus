﻿using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Contracts.Services;
using ECampus.Core.Metadata;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Shared.DataContainers;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Services.Services;

[Inject(typeof(ITimetableService))]
public class TimetableService : ITimetableService
{
    private readonly IMapper _mapper;
    private readonly IUpdateValidator<ClassDto> _validator;
    private readonly IDataAccessManager _dataAccess;

    public TimetableService(IMapper mapper, IUpdateValidator<ClassDto> validator, IDataAccessManager dataAccess)
    {
        _mapper = mapper;
        _validator = validator;
        _dataAccess = dataAccess;
    }

    public async Task<Timetable> GetTimetableForAuditoryAsync(int auditoryId)
    {
        var auditory = await _dataAccess.GetByIdAsync<Auditory>(auditoryId);
        return new Timetable(_mapper.Map<List<ClassDto>>(await _dataAccess
            .GetByParameters<Class, AuditoryTimetableParameters>(new AuditoryTimetableParameters
                { AuditoryId = auditoryId }).ToListAsync()))
        {
            Auditory = _mapper.Map<AuditoryDto>(auditory)
        };
    }

    public async Task<Timetable> GetTimetableForGroupAsync(int groupId)
    {
        var group = await _dataAccess.GetByIdAsync<Group>(groupId);
        return new Timetable(_mapper.Map<List<ClassDto>>(await _dataAccess
            .GetByParameters<Class, GroupTimetableParameters>(new GroupTimetableParameters { GroupId = groupId })
            .ToListAsync()))
        {
            Group = _mapper.Map<GroupDto>(group)
        };
    }

    public async Task<Timetable> GetTimetableForTeacherAsync(int teacherId)
    {
        var teacher = await _dataAccess.GetByIdAsync<Group>(teacherId);
        return new Timetable(_mapper.Map<List<ClassDto>>(await _dataAccess
            .GetByParameters<Class, TeacherTimetableParameters>(
                new TeacherTimetableParameters { TeacherId = teacherId }).ToListAsync()))
        {
            Teacher = _mapper.Map<TeacherDto>(teacher)
        };
    }

    public Task<ValidationResult> ValidateAsync(ClassDto @class)
    {
        return _validator.ValidateAsync(@class);
    }
}