using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.DataUpdate;

public class UserUpdate : IDataUpdate<User>
{
    private readonly IRelationshipsDataAccess<User, Auditory, UserAuditory> _userAuditoryRelations;
    private readonly IRelationshipsDataAccess<User, Group, UserGroup> _userGroupRelations;
    private readonly IRelationshipsDataAccess<User, Teacher, UserTeacher> _userTeacherRelations;
    private readonly IDataUpdate<User> _baseUpdate;

    public UserUpdate(IDataUpdate<User> baseUpdate, IRelationshipsDataAccess<User, Teacher, UserTeacher>
        userTeacherRelations, IRelationshipsDataAccess<User, Group, UserGroup> userGroupRelations,
        IRelationshipsDataAccess<User, Auditory, UserAuditory> userAuditoryRelations)
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