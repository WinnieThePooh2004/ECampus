namespace ECampus.Services.Contracts.Services;

public interface IUserRelationsService
{
    Task SaveAuditory(int userId, int auditoryId, CancellationToken token = default);
    Task RemoveSavedAuditory(int userId, int auditoryId, CancellationToken token = default);
    Task SaveGroup(int userId, int groupId, CancellationToken token = default);
    Task RemoveSavedGroup(int userId, int groupId, CancellationToken token = default);
    Task SaveTeacher(int userId, int teacherId, CancellationToken token = default);
    Task RemoveSavedTeacher(int userId, int teacherId, CancellationToken token = default);
}