using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Interfaces.Domain;

namespace UniversityTimetable.Domain.Services;

public class ClassService : IClassService
{
    private readonly ILogger<ClassService> _logger;
    private readonly ITimetableDataAccessFacade _repository;
    private readonly IMapper _mapper;
    private readonly IBaseService<ClassDto> _baseService;
    private readonly IUniversalValidator<ClassDto> _validator;

    public ClassService(ILogger<ClassService> logger, ITimetableDataAccessFacade repository, IMapper mapper,
        IBaseService<ClassDto> baseService, IUniversalValidator<ClassDto> validator)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _baseService = baseService;
        _validator = validator;
    }

    public Task<ClassDto> CreateAsync(ClassDto entity)
        => _baseService.CreateAsync(entity);

    public Task DeleteAsync(int? id)
        => _baseService.DeleteAsync(id);

    public Task<ClassDto> GetByIdAsync(int? id)
        => _baseService.GetByIdAsync(id);

    public async Task<Timetable> GetTimetableForAuditoryAsync(int? auditoryId)
    {
        if (auditoryId is null)
        {
            throw new NullIdException();
        }

        _logger.LogInformation("Getting timetable for group with id={Id}", auditoryId);
        var timetable = await _repository.GetTimetableForAuditoryAsync((int)auditoryId);
        return new Timetable(_mapper.Map<List<ClassDto>>(timetable.Classes))
        {
            Auditory = _mapper.Map<AuditoryDto>(timetable.Auditory)
        };
    }

    public async Task<Timetable> GetTimetableForGroupAsync(int? groupId)
    {
        if (groupId is null)
        {
            throw new NullIdException();
        }

        _logger.LogInformation("Getting timetable for group with id={Id}", groupId);
        var timetable = await _repository.GetTimetableForGroupAsync((int)groupId);
        return new Timetable(_mapper.Map<List<ClassDto>>(timetable.Classes))
        {
            Group = _mapper.Map<GroupDto>(timetable.Group),
        };
    }

    public async Task<Timetable> GetTimetableForTeacherAsync(int? teacherId)
    {
        if (teacherId is null)
        {
            throw new NullIdException();
        }

        _logger.LogInformation("Getting timetable for teacher with id={Id}", teacherId);
        var timetable = await _repository.GetTimetableForTeacherAsync((int)teacherId);
        return new Timetable(_mapper.Map<List<ClassDto>>(timetable.Classes))
        {
            Teacher = _mapper.Map<TeacherDto>(timetable.Teacher)
        };
    }

    public Task<ClassDto> UpdateAsync(ClassDto entity)
        => _baseService.UpdateAsync(entity);

    public Task<List<KeyValuePair<string, string>>> ValidateAsync(ClassDto @class)
    {
        return _validator.ValidateAsync(@class);
    }
}