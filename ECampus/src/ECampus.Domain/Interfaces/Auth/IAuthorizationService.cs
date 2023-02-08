using ECampus.Shared.DataTransferObjects;

namespace ECampus.Domain.Interfaces.Auth;

public interface IAuthorizationService
{
    Task<LoginResult> Login(LoginDto login, CancellationToken token = default);
}