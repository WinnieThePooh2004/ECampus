namespace ECampus.Shared.Interfaces.DataAccess;

public interface ITaskSubmissionRepository
{
    public Task UpdateContent(int submissionId, string content);
    public Task UpdateMark(int submissionId, int mark);
}