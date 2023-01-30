using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Shared.Interfaces.DataAccess;

public interface IPasswordChangeDataAccess
{
    Task<User> ChangePassword(PasswordChangeDto passwordChange);
}