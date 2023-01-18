namespace UniversityTimetable.FrontEnd.Requests.Interfaces;

public interface IPasswordChangeRequests
{
    Task ChangePassword(PasswordChangeDto passwordChange);
}