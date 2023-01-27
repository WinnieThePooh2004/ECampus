using ECampus.Shared.Models;

namespace ECampus.Shared.Interfaces.DataAccess;

public interface ITaskSubmissionRepository
{
    Task<TaskSubmission> UpdateContent(int submissionId, string content);
    Task<TaskSubmission> UpdateMark(int submissionId, int mark);
    Task<TaskSubmission> GetByIdAsync(int id);
    Task<TaskSubmission> GetByStudentAndCourse(int studentId, int courseTaskId);
}