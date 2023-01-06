using Microsoft.AspNetCore.Authentication;

namespace UniversityTimetable.FrontEnd.Auth;

public interface IAuthService
{
    Task Login(string email, string password, AuthenticationProperties properties = default!);
}