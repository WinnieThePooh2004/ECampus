using ECampus.Shared.DataTransferObjects;

namespace ECampus.FrontEnd.Requests.Interfaces;

public interface ITaskSubmissionRequests
{
    Task UpdateMarkAsync(int taskSubmissionId, int mark);
    Task UpdateContextAsync(int taskSubmissionId, string content);
    Task<TaskSubmissionDto> GetByCourseTaskAsync(int courseTaskId);
}