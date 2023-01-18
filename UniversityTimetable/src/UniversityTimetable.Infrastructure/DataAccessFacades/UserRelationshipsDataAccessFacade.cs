using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Attributes;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.DataAccessFacades;

[Inject(typeof(IUserRelationsDataAccessFacade))]
public class UserRelationshipsDataAccessFacade : IUserRelationsDataAccessFacade
{
    private readonly IRelationsDataAccess _relationsDataAccess;
    private readonly DbContext _context;

    public UserRelationshipsDataAccessFacade(IRelationsDataAccess relationsDataAccess, DbContext context)
    {
        _relationsDataAccess = relationsDataAccess;
        _context = context;
    }

    public Task SaveAuditory(int userId, int auditoryId)
    {
        return _relationsDataAccess.CreateRelation<UserAuditory, User, Auditory>(userId, auditoryId, _context);
    }

    public Task RemoveSavedAuditory(int userId, int auditoryId)
    {
        return _relationsDataAccess.DeleteRelation<UserAuditory, User, Auditory>(userId, auditoryId, _context);
    }

    public Task SaveGroup(int userId, int groupId)
    {
        return _relationsDataAccess.CreateRelation<UserGroup, User, Group>(userId, groupId, _context);
    }

    public Task RemoveSavedGroup(int userId, int groupId)
    {
        return _relationsDataAccess.DeleteRelation<UserGroup, User, Group>(userId, groupId, _context);
    }

    public Task SaveTeacher(int userId, int teacherId)
    {
        return _relationsDataAccess.CreateRelation<UserTeacher, User, Teacher>(userId, teacherId, _context);
    }

    public Task RemoveSavedTeacher(int userId, int teacherId)
    {
        return _relationsDataAccess.DeleteRelation<UserTeacher, User, Teacher>(userId, teacherId, _context);
    }
}