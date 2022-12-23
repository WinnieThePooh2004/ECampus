using AutoMapper;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Domain.Services;

public class BaseTeacherService : IBaseService<TeacherDto>
{
    private readonly IBaseService<TeacherDto> _baseService;
    private readonly IRelationshipsRepository<Teacher, Subject, SubjectTeacher> _relationships;
    private readonly IMapper _mapper;
    private readonly IBaseRepository<Teacher> _repository;

    public BaseTeacherService(IBaseService<TeacherDto> baseService, IRelationshipsRepository<Teacher, Subject, SubjectTeacher> relationships,
        IMapper mapper, IBaseRepository<Teacher> repository)
    {
        _baseService = baseService;
        _relationships = relationships;
        _mapper = mapper;
        _repository = repository;
    }

    public Task<TeacherDto> GetByIdAsync(int? id) => _baseService.GetByIdAsync(id);

    public async Task<TeacherDto> CreateAsync(TeacherDto entity)
    {
        var model = _mapper.Map<Teacher>(entity);
        _relationships.CreateRelationModels(model);
        return _mapper.Map<TeacherDto>(await _repository.CreateAsync(model));
    }

    public async Task<TeacherDto> UpdateAsync(TeacherDto entity)
    {
        var model = _mapper.Map<Teacher>(entity);
        await _relationships.UpdateRelations(model);
        return _mapper.Map<TeacherDto>(await _repository.UpdateAsync(model));
    }

    public Task DeleteAsync(int? id) => _baseService.DeleteAsync(id);
}