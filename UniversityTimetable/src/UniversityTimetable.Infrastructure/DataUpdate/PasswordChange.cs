using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.DataUpdate;

public class PasswordChange : IPasswordChange
{
    private readonly ApplicationDbContext _context;

    public PasswordChange(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> ChangePassword(PasswordChangeDto passwordChange)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == passwordChange.UserId)
                   ?? throw new ObjectNotFoundByIdException(typeof(User), passwordChange.UserId);
        
        user.Password = passwordChange.NewPassword;
        await _context.SaveChangesAsync();
        return user;
    }
}