using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.ValidationRepositories;

public class UserValidationRepository : IValidationRepository<User>
{
    private readonly ApplicationDbContext _context;

    public UserValidationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<User> LoadRequiredDataForCreate(User model)
    {
        return _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == model.Id);
    }

    public Task<User> LoadRequiredDataForUpdate(User model)
    {
        return _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == model.Id);
    }

    async Task<Dictionary<string, string>> IValidationRepository<User>.ValidateUpdateOnDataBaseLevel(User model)
    {
        var errors = new Dictionary<string, string>();
        if (await _context.Users.AsNoTracking().AnyAsync(u => u.Id != model.Id && u.Username == model.Username))
        {
            errors.Add(nameof(model.Username), "This username is already used");
        }
        return errors;
    }

    async Task<Dictionary<string, string>> IValidationRepository<User>.ValidateCreateOnDataBaseLevel(User user)
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