using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.Interfaces.DataAccess;

public interface IPasswordChange
{
    Task<User> ChangePassword(PasswordChangeDto passwordChange);
}