using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.DataUpdateServices;

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