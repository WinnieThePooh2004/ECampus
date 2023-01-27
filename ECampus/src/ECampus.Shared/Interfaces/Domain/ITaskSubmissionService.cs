using ECampus.Shared.DataTransferObjects;

namespace ECampus.Shared.Interfaces.Domain;

public interface ITaskSubmissionService
{
    public Task UpdateContent(int submissionId, string content);
    public Task UpdateMark(int submissionId, int mark);
    Task<TaskSubmissionDto> GetByIdAsync(int id);
    Task<TaskSubmissionDto> GetByCourse(int courseTaskId);
}