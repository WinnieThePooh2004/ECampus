using ECampus.Core.Installers;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Domain.Entities;
using ECampus.Domain.Entities.RelationEntities;
using ECampus.Services.Contracts.Services;

namespace ECampus.Services.Services;

[Inject(typeof(IUserRelationsService))]
public class UserRelationsService : IUserRelationsService
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IRelationsDataAccess _relationsDataAccess;
    private readonly IDataAccessFacade _dataAccess;

    public UserRelationsService(IAuthenticationService authenticationService, IRelationsDataAccess relationsDataAccess,
        IDataAccessFacade dataAccess)
    {
        _authenticationService = authenticationService;
        _relationsDataAccess = relationsDataAccess;
        _dataAccess = dataAccess;
    }

    public async Task SaveAuditory(int userId, int auditoryId, CancellationToken token = default)
    {
        _authenticationService.VerifyUser(userId);
        _relationsDataAccess.CreateRelation<User, Auditory, UserAuditory>(userId, auditoryId);
        await _dataAccess.SaveChangesAsync(token);
    }

    public async Task RemoveSavedAuditory(int userId, int auditoryId, CancellationToken token = default)
    {
        _authenticationService.VerifyUser(userId);
        _relationsDataAccess.DeleteRelation<User, Auditory, UserAuditory>(userId, auditoryId);
        await _dataAccess.SaveChangesAsync(token);
    }

    public async Task SaveGroup(int userId, int groupId, CancellationToken token = default)
    {
        _authenticationService.VerifyUser(userId);
        _relationsDataAccess.CreateRelation<User, Group, UserGroup>(userId, groupId);
        await _dataAccess.SaveChangesAsync(token);
    }

    public async Task RemoveSavedGroup(int userId, int groupId, CancellationToken token = default)
    {
        _authenticationService.VerifyUser(userId);
        _relationsDataAccess.DeleteRelation<User, Group, UserGroup>(userId, groupId);
        await _dataAccess.SaveChangesAsync(token);
    }

    public async Task SaveTeacher(int userId, int teacherId, CancellationToken token = default)
    {
        _authenticationService.VerifyUser(userId);
        _relationsDataAccess.CreateRelation<User, Teacher, UserTeacher>(userId, teacherId);
        await _dataAccess.SaveChangesAsync(token);
    }

    public async Task RemoveSavedTeacher(int userId, int teacherId, CancellationToken token = default)
    {
        _authenticationService.VerifyUser(userId);
        _relationsDataAccess.DeleteRelation<User, Teacher, UserTeacher>(userId, teacherId);
        await _dataAccess.SaveChangesAsync(token);
    }
}