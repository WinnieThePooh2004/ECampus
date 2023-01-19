﻿using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Metadata;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.DataAccessFacades;

[Inject(typeof(IUserRelationsDataAccessFacade))]
public class UserRelationshipsDataAccessFacade : IUserRelationsDataAccessFacade
{
    private readonly IRelationsDataAccess<User, Auditory, UserAuditory> _userAuditoryRelations;
    private readonly IRelationsDataAccess<User, Group, UserGroup> _userGroupRelations;
    private readonly IRelationsDataAccess<User, Teacher, UserTeacher> _userTeacherRelations;
    private readonly DbContext _context;

    public UserRelationshipsDataAccessFacade(DbContext context,
        IRelationsDataAccess<User, Auditory, UserAuditory> userAuditoryRelations,
        IRelationsDataAccess<User, Group, UserGroup> userGroupRelations,
        IRelationsDataAccess<User, Teacher, UserTeacher> userTeacherRelations)
    {
        _context = context;
        _userAuditoryRelations = userAuditoryRelations;
        _userGroupRelations = userGroupRelations;
        _userTeacherRelations = userTeacherRelations;
    }

    public Task SaveAuditory(int userId, int auditoryId)
    {
        return _userAuditoryRelations.CreateRelation(userId, auditoryId, _context);
    }

    public Task RemoveSavedAuditory(int userId, int auditoryId)
    {
        return _userAuditoryRelations.DeleteRelation(userId, auditoryId, _context);
    }

    public Task SaveGroup(int userId, int groupId)
    {
        return _userGroupRelations.CreateRelation(userId, groupId, _context);
    }

    public Task RemoveSavedGroup(int userId, int groupId)
    {
        return _userGroupRelations.DeleteRelation(userId, groupId, _context);
    }

    public Task SaveTeacher(int userId, int teacherId)
    {
        return _userTeacherRelations.CreateRelation(userId, teacherId, _context);
    }

    public Task RemoveSavedTeacher(int userId, int teacherId)
    {
        return _userTeacherRelations.DeleteRelation(userId, teacherId, _context);
    }
}