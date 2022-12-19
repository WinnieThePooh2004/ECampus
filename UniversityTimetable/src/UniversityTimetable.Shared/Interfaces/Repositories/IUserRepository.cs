using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public Task<Dictionary<string, string>> ValidateAsync(User user);
    }
}
