using ECampus.Shared.Models;

namespace ECampus.Shared.Interfaces.DataAccess;

public interface IUserRolesRepository
{
    Task<User> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
}