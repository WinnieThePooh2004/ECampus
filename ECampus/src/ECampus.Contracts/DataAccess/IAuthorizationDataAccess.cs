using ECampus.Shared.Models;

namespace ECampus.Contracts.DataAccess;

public interface IAuthorizationDataAccess
{
    Task<User> GetByEmailAsync(string email);
}