using Microsoft.AspNetCore.Authentication;

namespace ECampus.FrontEnd.Auth;

public interface IAuthService
{
    Task Login(string email, string password, AuthenticationProperties properties = default!);
}