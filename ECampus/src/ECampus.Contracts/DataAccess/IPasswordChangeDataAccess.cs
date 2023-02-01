using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Contracts.DataAccess;

public interface IPasswordChangeDataAccess
{
    Task<User> ChangePassword(PasswordChangeDto passwordChange);
}