using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Metadata;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.DataAccessFacades;

[Inject(typeof(IUserDataAccessFacade))]
public class UserDataAccessFacade : IUserDataAccessFacade
{
    private readonly DbContext _context;
    private readonly IPasswordChange _passwordChange;
    private readonly IRelationsDataAccess _relationsDataAccess;
    
    public UserDataAccessFacade(DbContext context,
        IPasswordChange passwordChange, IRelationsDataAccess relationsDataAccess)
    {
        _context = context;
        _passwordChange = passwordChange;
        _relationsDataAccess = relationsDataAccess;
    }
    
    public async Task<User> ChangePassword(PasswordChangeDto passwordChange)
    {
        return await _passwordChange.ChangePassword(passwordChange, _context);
    }
}