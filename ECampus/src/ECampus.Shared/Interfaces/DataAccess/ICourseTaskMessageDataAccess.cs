namespace ECampus.Shared.Interfaces.DataAccess;

public interface ICourseTaskMessageDataAccess
{
    Task<(string courseName, List<string> studentEmails)> LoadDataForSendMessage(int courseId);
}