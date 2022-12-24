using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.DataUpdate;

public class UserUpdate : IDataUpdate<User>
{
    private readonly IRelationshipsRepository<User, Auditory, UserAuditory> _userAuditoryRelations;
    private readonly IRelationshipsRepository<User, Group, UserGroup> _userGroupRelations;
    private readonly IRelationshipsRepository<User, Teacher, UserTeacher> _userTeacherRelations;
    private readonly IDataUpdate<User> _baseUpdate;

    public UserUpdate(IDataUpdate<User> baseUpdate, IRelationshipsRepository<User, Teacher, UserTeacher>
        userTeacherRelations, IRelationshipsRepository<User, Group, UserGroup> userGroupRelations,
        IRelationshipsRepository<User, Auditory, UserAuditory> userAuditoryRelations)
    {
        _baseUpdate = baseUpdate;
        _userTeacherRelations = userTeacherRelations;
        _userGroupRelations = userGroupRelations;
        _userAuditoryRelations = userAuditoryRelations;
    }


    public async Task<User> UpdateAsync(User model, DbContext context)
    {
        await _userAuditoryRelations.UpdateRelations(model);
        await _userGroupRelations.UpdateRelations(model);
        await _userTeacherRelations.UpdateRelations(model);
        return await _baseUpdate.UpdateAsync(model, context);
    }
}