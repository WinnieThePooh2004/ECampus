using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Interfaces.Data.Validation;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.ValidationDataAccess;

public class UserDataValidator : IDataValidator<User>
{
    private readonly ApplicationDbContext _context;

    public UserDataValidator(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> LoadRequiredDataForCreate(User model)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == model.Id)
               ?? throw new ObjectNotFoundByIdException(typeof(User), model.Id);
    }

    public async Task<User> LoadRequiredDataForUpdate(User model)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == model.Id)
               ?? throw new ObjectNotFoundByIdException(typeof(User), model.Id);
    }

    public async Task<Dictionary<string, string>> ValidateUpdate(User model)
    {
        var errors = new Dictionary<string, string>();
        if (await _context.Users.AsNoTracking().AnyAsync(u => u.Id != model.Id && u.Username == model.Username))
        {
            errors.Add(nameof(model.Username), "This username is already used");
        }

        return errors;
    }

    public async Task<Dictionary<string, string>> ValidateCreate(User user)
    {
        var errors = new Dictionary<string, string>();
        if (await _context.Users.AnyAsync(u => u.Email == user.Email && u.Id != user.Id))
        {
            errors.Add(nameof(user.Email), "This email is already user");
        }

        if (await _context.Users.AnyAsync(u => u.Username == user.Username))
        {
            errors.Add(nameof(user.Email), "This username is already user");
        }

        return errors;
    }
}