using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public Task<Dictionary<string, string>> ValidateCreateAsync(User user);
        public Task<Dictionary<string, string>> ValidateUpdateAsync(User user);
    }
}
