namespace UniversityTimetable.FrontEnd.Requests.Interfaces;

public interface IAuthInterface
{
    Task<UserDTO> LoginAsync(LoginDTO login);
    Task Logout();
}