using ECampus.Shared.Models;

namespace ECampus.Shared.Interfaces.DataAccess;

public interface ITaskSubmissionRepository
{
    Task<TaskSubmission> UpdateContentAsync(int submissionId, string content);
    Task<TaskSubmission> UpdateMarkAsync(int submissionId, int mark);
    Task<TaskSubmission> GetByIdAsync(int id);
    Task<TaskSubmission> GetByStudentAndCourseAsync(int studentId, int courseTaskId);
}