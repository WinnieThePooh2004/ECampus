namespace ECampus.Contracts.DataAccess;

public interface ICourseTaskMessageDataAccess
{
    Task<(string CourseName, List<string> StudentEmails)> LoadDataForSendMessage(int courseId);
}