using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.Interfaces.Authorization;

public interface IAuthRepository
{
    Task<User> GetByEmailAsync(string email);
}