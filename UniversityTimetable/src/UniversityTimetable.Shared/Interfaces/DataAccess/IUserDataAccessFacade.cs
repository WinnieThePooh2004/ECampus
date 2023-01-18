using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.Interfaces.DataAccess;

public interface IUserDataAccessFacade
{
    Task<User> ChangePassword(PasswordChangeDto passwordChange);
}