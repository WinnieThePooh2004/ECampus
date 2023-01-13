using UniversityTimetable.Shared.Auth;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Shared.Interfaces.Auth;

public interface IAuthorizationService
{
    Task<LoginResult> Login(LoginDto login);
}