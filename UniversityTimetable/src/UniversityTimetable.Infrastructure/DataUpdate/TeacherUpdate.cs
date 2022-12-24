using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.DataUpdate;

public class TeacherUpdate : IDataUpdate<Teacher>
{
    private readonly IRelationshipsRepository<Teacher, Subject, SubjectTeacher> _relationships;
    private readonly IDataUpdate<Teacher> _baseUpdate;

    public TeacherUpdate(IDataUpdate<Teacher> baseUpdate, IRelationshipsRepository<Teacher, Subject, SubjectTeacher> relationships)
    {
        _baseUpdate = baseUpdate;
        _relationships = relationships;
    }

    public async Task<Teacher> UpdateAsync(Teacher model, DbContext context)
    {
        await _relationships.UpdateRelations(model);
        return await _baseUpdate.UpdateAsync(model, context);
    }
}