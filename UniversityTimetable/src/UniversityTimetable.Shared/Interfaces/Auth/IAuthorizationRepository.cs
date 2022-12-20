using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.Interfaces.Auth;

public interface IAuthorizationRepository
{
    Task<User> GetByEmailAsync(string email);
}