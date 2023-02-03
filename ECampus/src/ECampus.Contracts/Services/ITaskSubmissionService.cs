using ECampus.Shared.DataTransferObjects;

namespace ECampus.Contracts.Services;

public interface ITaskSubmissionService
{
    public Task<TaskSubmissionDto> UpdateContentAsync(int submissionId, string content);
    public Task<TaskSubmissionDto> UpdateMarkAsync(int submissionId, int mark);
    Task<TaskSubmissionDto> GetByIdAsync(int id);
    Task<TaskSubmissionDto> GetByCourse(int courseTaskId);
}