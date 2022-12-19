using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Shared.Interfaces.Services
{
    public interface IUserService : IBaseService<UserDTO>
    {
        Task<Dictionary<string, string>> ValidateAsync(UserDTO userDTO);
    }
}
