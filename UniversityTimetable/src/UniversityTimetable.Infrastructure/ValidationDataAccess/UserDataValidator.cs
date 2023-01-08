using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.ValidationDataAccess;

public class UserDataValidator : IDataValidator<User>, IValidationDataAccess<User>
{
    private readonly ApplicationDbContext _context;

    public UserDataValidator(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> LoadRequiredDataForCreateAsync(User model)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == model.Id)
               ?? throw new ObjectNotFoundByIdException(typeof(User), model.Id);
    }

    public async Task<User> LoadRequiredDataForUpdateAsync(User model)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == model.Id)
               ?? throw new ObjectNotFoundByIdException(typeof(User), model.Id);
    }

    public async Task<List<KeyValuePair<string, string>>> ValidateUpdate(User model)
    {
        var errors = new List<KeyValuePair<string, string>>();
        if (await _context.Users.AsNoTracking().AnyAsync(u => u.Id != model.Id && u.Username == model.Username))
        {
            errors.Add(KeyValuePair.Create(nameof(model.Username), "This username is already used"));
        }

        return errors;
    }

    public async Task<List<KeyValuePair<string, string>>> ValidateCreate(User user)
    {
        var errors = new List<KeyValuePair<string, string>>();
        if (await _context.Users.AnyAsync(u => u.Email == user.Email && u.Id != user.Id))
        {
            errors.Add(KeyValuePair.Create(nameof(user.Email), "This email is already user"));
        }

        if (await _context.Users.AnyAsync(u => u.Username == user.Username))
        {
            errors.Add(KeyValuePair.Create(nameof(user.Username), "This username is already user"));
        }

        return errors;
    }
}