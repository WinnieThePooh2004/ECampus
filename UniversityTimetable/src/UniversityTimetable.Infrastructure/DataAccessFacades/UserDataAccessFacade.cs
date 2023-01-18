using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Metadata;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.DataAccessFacades;

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