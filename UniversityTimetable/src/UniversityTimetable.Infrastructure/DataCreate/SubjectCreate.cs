using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.DataCreate;

public class SubjectCreate : IDataCreate<Subject>
{
    private readonly IRelationshipsRepository<Subject, Teacher, SubjectTeacher> _relationships;
    private readonly IDataCreate<Subject> _baseCreate;

    public SubjectCreate(IDataCreate<Subject> baseCreate, IRelationshipsRepository<Subject, Teacher, SubjectTeacher> relationships)
    {
        _baseCreate = baseCreate;
        _relationships = relationships;
    }

    public async Task<Subject> CreateAsync(Subject model, DbContext context)
    {
        _relationships.CreateRelationModels(model);
        return await _baseCreate.CreateAsync(model, context);
    }
}