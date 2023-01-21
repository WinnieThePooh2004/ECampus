using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Shared.Interfaces.DataAccess;

public interface IPasswordChange
{
    Task<User> ChangePassword(PasswordChangeDto passwordChange, DbContext context);
}