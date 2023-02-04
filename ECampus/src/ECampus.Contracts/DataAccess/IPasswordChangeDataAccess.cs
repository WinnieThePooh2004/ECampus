using ECampus.Shared.Models;

namespace ECampus.Contracts.DataAccess;

public interface IPasswordChangeDataAccess
{
    Task<User> GetUserAsync(int userId);
    Task<bool> SaveChangesAsync();
}