using ECampus.Shared.Models;

namespace ECampus.Shared.Interfaces.DataAccess;

public interface ITaskSubmissionRepository
{
    public Task UpdateContent(int submissionId, string content);
    public Task UpdateMark(int submissionId, int mark);
    Task<TaskSubmission> GetByIdAsync(int id);
    Task<TaskSubmission> GetByStudentAndCourse(int studentId, int courseTaskId);
}