using ECampus.Shared.Models;

namespace ECampus.Shared.Interfaces.Auth;

public interface IAuthorizationDataAccess
{
    Task<User> GetByEmailAsync(string email);
}