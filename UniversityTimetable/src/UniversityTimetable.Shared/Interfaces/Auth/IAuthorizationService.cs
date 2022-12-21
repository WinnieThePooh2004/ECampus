using Microsoft.AspNetCore.Http;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Shared.Interfaces.Auth;

public interface IAuthorizationService
{
    Task<UserDto> Login(LoginDto login, HttpContext context);
    Task Logout(HttpContext context);
}