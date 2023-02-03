using ECampus.Contracts.DataAccess;
using ECampus.Contracts.Services;
using ECampus.Core.Metadata;
using ECampus.Domain.Interfaces.Auth;
using ECampus.Shared.Models;
using ECampus.Shared.Models.RelationModels;

namespace ECampus.Services.Services;

[Inject(typeof(IUserRelationsService))]
public class UserRelationsService : IUserRelationsService
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IRelationsDataAccess _relationsDataAccess;

    public UserRelationsService(IAuthenticationService authenticationService, IRelationsDataAccess relationsDataAccess)
    {
        _authenticationService = authenticationService;
        _relationsDataAccess = relationsDataAccess;
    }

    public Task SaveAuditory(int userId, int auditoryId)
    {
        _authenticationService.VerifyUser(userId);
        return _relationsDataAccess.CreateRelation<User, Auditory, UserAuditory>(userId, auditoryId);
    }

    public Task RemoveSavedAuditory(int userId, int auditoryId)
    {
        _authenticationService.VerifyUser(userId);
        return _relationsDataAccess.DeleteRelation<User, Auditory, UserAuditory>(userId, auditoryId);
    }

    public Task SaveGroup(int userId, int groupId)
    {
        _authenticationService.VerifyUser(userId);
        return _relationsDataAccess.CreateRelation<User, Group, UserGroup>(userId, groupId);
    }

    public Task RemoveSavedGroup(int userId, int groupId)
    {
        _authenticationService.VerifyUser(userId);
        return _relationsDataAccess.DeleteRelation<User, Group, UserGroup>(userId, groupId);
    }

    public Task SaveTeacher(int userId, int teacherId)
    {
        _authenticationService.VerifyUser(userId);
        return _relationsDataAccess.CreateRelation<User, Teacher, UserTeacher>(userId, teacherId);
    }

    public Task RemoveSavedTeacher(int userId, int teacherId)
    {
        _authenticationService.VerifyUser(userId);
        return _relationsDataAccess.DeleteRelation<User, Teacher, UserTeacher>(userId, teacherId);
    }
}