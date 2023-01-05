using UniversityTimetable.Shared.Auth;

namespace UniversityTimetable.FrontEnd.Requests.Interfaces;

public interface IAuthRequests
{
    Task<LoginResult> LoginAsync(LoginDto login);
    Task LogoutAsync();
}