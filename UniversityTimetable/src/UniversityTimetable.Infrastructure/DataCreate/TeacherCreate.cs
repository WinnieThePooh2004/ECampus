using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.DataCreate;

public class TeacherCreate : IDataCreate<Teacher>
{
    private readonly IRelationshipsRepository<Teacher, Subject, SubjectTeacher> _relationships;
    private readonly IDataCreate<Teacher> _baseCreate;

    public TeacherCreate(IDataCreate<Teacher> baseCreate, IRelationshipsRepository<Teacher, Subject, SubjectTeacher> relationships)
    {
        _baseCreate = baseCreate;
        _relationships = relationships;
    }

    public async Task<Teacher> CreateAsync(Teacher model, DbContext context)
    {
        _relationships.CreateRelationModels(model);
        return await _baseCreate.CreateAsync(model, context);
    }
}