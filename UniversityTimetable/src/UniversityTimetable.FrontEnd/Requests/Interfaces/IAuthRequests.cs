namespace UniversityTimetable.FrontEnd.Requests.Interfaces;

public interface IAuthRequests
{
    Task<UserDto> LoginAsync(LoginDto login);
    Task LogoutAsync();
}