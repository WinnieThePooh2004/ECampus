using ECampus.Shared.Models;

namespace ECampus.Contracts.DataAccess;

public interface IUserRolesRepository
{
    Task<User> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<User> DeleteAsync(int id);
}