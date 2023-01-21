using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Shared.Interfaces.DataAccess;

public interface IUserDataAccessFacade
{
    Task<User> ChangePassword(PasswordChangeDto passwordChange);
}