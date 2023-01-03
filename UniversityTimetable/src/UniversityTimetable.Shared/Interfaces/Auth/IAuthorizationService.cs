using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Shared.Interfaces.Auth;

public interface IAuthorizationService
{
    Task<UserDto> Login(LoginDto login);
    Task Logout();
}