using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataUpdateServices;

[Inject(typeof(IPasswordChange))]
public class PasswordChange : IPasswordChange
{
    public async Task<User> ChangePassword(PasswordChangeDto passwordChange, DbContext context)
    {
        var user = await context.Set<User>().FirstOrDefaultAsync(user => user.Id == passwordChange.UserId)
                   ?? throw new ObjectNotFoundByIdException(typeof(User), passwordChange.UserId);
        
        user.Password = passwordChange.NewPassword;
        await context.SaveChangesAsync();
        return user;
    }
}