using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.DataCreate;

public class UserCreate : IDataCreate<User>
{
    private readonly IRelationshipsDataAccess<User, Auditory, UserAuditory> _userAuditoryRelations;
    private readonly IRelationshipsDataAccess<User, Group, UserGroup> _userGroupRelations;
    private readonly IRelationshipsDataAccess<User, Teacher, UserTeacher> _userTeacherRelations;
    private readonly IDataCreate<User> _baseCreate;

    public UserCreate(IDataCreate<User> baseCreate, IRelationshipsDataAccess<User, Teacher, UserTeacher>
            userTeacherRelations, IRelationshipsDataAccess<User, Group, UserGroup> userGroupRelations,
        IRelationshipsDataAccess<User, Auditory, UserAuditory> userAuditoryRelations)
    {
        _baseCreate = baseCreate;
        _userTeacherRelations = userTeacherRelations;
        _userGroupRelations = userGroupRelations;
        _userAuditoryRelations = userAuditoryRelations;
    }


    public async Task<User> CreateAsync(User model, DbContext context)
    {
        _userAuditoryRelations.CreateRelationModels(model);
        _userGroupRelations.CreateRelationModels(model);
        _userTeacherRelations.CreateRelationModels(model);
        return await _baseCreate.CreateAsync(model, context);
    }
}