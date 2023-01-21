using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;

namespace ECampus.FrontEnd.Requests.Interfaces;

public interface IAuthRequests
{
    Task<LoginResult> LoginAsync(LoginDto login);
    Task LogoutAsync();
}