namespace ECampus.Shared.Interfaces.DataAccess;

public interface ICourseTaskMessageDataAccess
{
    Task<(string CourseName, List<string> StudentEmails)> LoadDataForSendMessage(int courseId);
}