using Microsoft.AspNetCore.Authentication;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.Extensions;

namespace UniversityTimetable.FrontEnd.Auth;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthRequests _requests;

    public AuthService(IHttpContextAccessor httpContextAccessor, IAuthRequests requests)
    {
        _httpContextAccessor = httpContextAccessor;
        _requests = requests;
    }

    public async Task Login(string email, string password, AuthenticationProperties properties = default!)
    {
        var login = new LoginDto { Email = email, Password = password };
        var user = await _requests.LoginAsync(login);
        await _httpContextAccessor.HttpContext?.SignInAsync(user, properties)!;
    }

}