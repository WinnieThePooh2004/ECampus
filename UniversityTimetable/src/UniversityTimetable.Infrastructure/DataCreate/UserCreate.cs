using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.DataCreate;

public class UserCreate : IDataCreate<User>
{
    private readonly IRelationshipsRepository<User, Auditory, UserAuditory> _userAuditoryRelations;
    private readonly IRelationshipsRepository<User, Group, UserGroup> _userGroupRelations;
    private readonly IRelationshipsRepository<User, Teacher, UserTeacher> _userTeacherRelations;
    private readonly IDataCreate<User> _baseCreate;

    public UserCreate(IDataCreate<User> baseCreate, IRelationshipsRepository<User, Teacher, UserTeacher>
            userTeacherRelations, IRelationshipsRepository<User, Group, UserGroup> userGroupRelations,
        IRelationshipsRepository<User, Auditory, UserAuditory> userAuditoryRelations)
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