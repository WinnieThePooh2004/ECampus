using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.DataUpdate;

public class SubjectUpdate : IDataUpdate<Subject>
{
    private readonly IRelationshipsDataAccess<Subject, Teacher, SubjectTeacher> _relationships;
    private readonly IDataUpdate<Subject> _baseUpdate;

    public SubjectUpdate(IRelationshipsDataAccess<Subject, Teacher, SubjectTeacher> relationships, IDataUpdate<Subject> baseUpdate)
    {
        _relationships = relationships;
        _baseUpdate = baseUpdate;
    }

    public async Task<Subject> UpdateAsync(Subject model, DbContext context)
    {
        await _relationships.UpdateRelations(model);
        return await _baseUpdate.UpdateAsync(model, context);
    }
}