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

    public Task SaveAuditory(int userId, int auditoryId, CancellationToken token = default)
    {
        _authenticationService.VerifyUser(userId);
        return _relationsDataAccess.CreateRelation<User, Auditory, UserAuditory>(userId, auditoryId, token);
    }

    public Task RemoveSavedAuditory(int userId, int auditoryId, CancellationToken token = default)
    {
        _authenticationService.VerifyUser(userId);
        return _relationsDataAccess.DeleteRelation<User, Auditory, UserAuditory>(userId, auditoryId, token);
    }

    public Task SaveGroup(int userId, int groupId, CancellationToken token = default)
    {
        _authenticationService.VerifyUser(userId);
        return _relationsDataAccess.CreateRelation<User, Group, UserGroup>(userId, groupId, token);
    }

    public Task RemoveSavedGroup(int userId, int groupId, CancellationToken token = default)
    {
        _authenticationService.VerifyUser(userId);
        return _relationsDataAccess.DeleteRelation<User, Group, UserGroup>(userId, groupId, token);
    }

    public Task SaveTeacher(int userId, int teacherId, CancellationToken token = default)
    {
        _authenticationService.VerifyUser(userId);
        return _relationsDataAccess.CreateRelation<User, Teacher, UserTeacher>(userId, teacherId, token);
    }

    public Task RemoveSavedTeacher(int userId, int teacherId, CancellationToken token = default)
    {
        _authenticationService.VerifyUser(userId);
        return _relationsDataAccess.DeleteRelation<User, Teacher, UserTeacher>(userId, teacherId, token);
    }
}