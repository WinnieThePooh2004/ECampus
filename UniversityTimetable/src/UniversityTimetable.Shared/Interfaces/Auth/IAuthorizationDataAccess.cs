using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.Interfaces.Auth;

public interface IAuthorizationDataAccess
{
    Task<User> GetByEmailAsync(string email);
}