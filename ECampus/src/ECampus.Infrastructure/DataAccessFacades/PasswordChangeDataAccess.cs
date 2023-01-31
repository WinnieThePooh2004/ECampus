using ECampus.Core.Metadata;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataAccessFacades;

[Inject(typeof(IPasswordChangeDataAccess))]
public class PasswordChangeDataAccess : IPasswordChangeDataAccess
{
    private readonly DbContext _context;

    public PasswordChangeDataAccess(DbContext context)
    {
        _context = context;
    }
    
    public async Task<User> ChangePassword(PasswordChangeDto passwordChange)
    {
        var user = await _context.Set<User>().FirstOrDefaultAsync(user => user.Id == passwordChange.UserId)
                   ?? throw new ObjectNotFoundByIdException(typeof(User), passwordChange.UserId);
        
        user.Password = passwordChange.NewPassword;
        await _context.SaveChangesAsync();
        return user;
    }
}