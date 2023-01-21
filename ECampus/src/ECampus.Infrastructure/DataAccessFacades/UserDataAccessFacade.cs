using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataAccessFacades;

[Inject(typeof(IUserDataAccessFacade))]
public class UserDataAccessFacade : IUserDataAccessFacade
{
    private readonly DbContext _context;
    private readonly IPasswordChange _passwordChange;

    public UserDataAccessFacade(DbContext context,
        IPasswordChange passwordChange)
    {
        _context = context;
        _passwordChange = passwordChange;
    }
    
    public async Task<User> ChangePassword(PasswordChangeDto passwordChange)
    {
        return await _passwordChange.ChangePassword(passwordChange, _context);
    }
}