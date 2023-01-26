namespace ECampus.Shared.Interfaces.Domain;

public interface ITaskSubmissionService
{
    public Task UpdateContent(int submissionId, string content);
    public Task UpdateMark(int submissionId, int mark);
}