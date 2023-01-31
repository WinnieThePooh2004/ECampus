using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;
using ECampus.Shared.Models.RelationModels;

namespace ECampus.Infrastructure.DataAccessFacades;

[Inject(typeof(IUserRelationsDataAccessFacade))]
public class UserRelationshipsDataAccessFacade : IUserRelationsDataAccessFacade
{
    private readonly IRelationsDataAccess<User, Auditory, UserAuditory> _userAuditoryRelations;
    private readonly IRelationsDataAccess<User, Group, UserGroup> _userGroupRelations;
    private readonly IRelationsDataAccess<User, Teacher, UserTeacher> _userTeacherRelations;
    private readonly ApplicationDbContext _context;

    public UserRelationshipsDataAccessFacade(ApplicationDbContext context,
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