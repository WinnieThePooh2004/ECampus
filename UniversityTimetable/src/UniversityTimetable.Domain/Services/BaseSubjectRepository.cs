using AutoMapper;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Domain.Services;

public class BaseSubjectService : IBaseService<SubjectDto>
{
    private readonly IBaseService<SubjectDto> _baseService;
    private readonly IRelationshipsRepository<Subject, Teacher, SubjectTeacher> _relationships;
    private readonly IMapper _mapper;
    private readonly IBaseRepository<Subject> _repository;

    public BaseSubjectService(IBaseService<SubjectDto> baseService, IRelationshipsRepository<Subject, Teacher, SubjectTeacher> relationships, IMapper mapper, IBaseRepository<Subject> repository)
    {
        _baseService = baseService;
        _relationships = relationships;
        _mapper = mapper;
        _repository = repository;
    }

    public Task<SubjectDto> GetByIdAsync(int? id) 
        => _baseService.GetByIdAsync(id);

    public async Task<SubjectDto> CreateAsync(SubjectDto entity)
    {
        var model = _mapper.Map<Subject>(entity);
        _relationships.CreateRelationModels(model);
        return _mapper.Map<SubjectDto>(await _repository.CreateAsync(model));
    }

    public async Task<SubjectDto> UpdateAsync(SubjectDto entity)
    {
        var model = _mapper.Map<Subject>(entity);
        await _relationships.UpdateRelations(model);
        return _mapper.Map<SubjectDto>(await _repository.UpdateAsync(model));
    }

    public Task DeleteAsync(int? id) 
        => _baseService.DeleteAsync(id);
}