namespace UniversityTimetable.FrontEnd.Requests.Interfaces;

public interface IAuthRequests
{
    Task<UserDTO> LoginAsync(LoginDTO login);
    Task LogoutAsync();
}