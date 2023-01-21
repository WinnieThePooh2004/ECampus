using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;

namespace ECampus.Shared.Interfaces.Auth;

public interface IAuthorizationService
{
    Task<LoginResult> Login(LoginDto login);
}