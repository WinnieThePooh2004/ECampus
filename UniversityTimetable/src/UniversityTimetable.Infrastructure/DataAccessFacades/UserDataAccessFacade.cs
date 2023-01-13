using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Attributes;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.DataAccessFacades;

[Inject(typeof(IUserDataAccessFacade))]
public class UserDataAccessFacade : IUserDataAccessFacade
{
    private readonly DbContext _context;
    private readonly IPasswordChange _passwordChange;
    private readonly IRelationsDataAccess _relationsDataAccess;
    
    public UserDataAccessFacade(DbContext context,
        IPasswordChange passwordChange, IRelationsDataAccess relationsDataAccess)
    {
        _context = context;
        _passwordChange = passwordChange;
        _relationsDataAccess = relationsDataAccess;
    }
    
    public async Task<User> ChangePassword(PasswordChangeDto passwordChange)
    {
        return await _passwordChange.ChangePassword(passwordChange, _context);
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