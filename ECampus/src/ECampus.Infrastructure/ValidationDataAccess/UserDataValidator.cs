using ECampus.Core.Metadata;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.ValidationDataAccess;

[Inject(typeof(IDataValidator<User>))]
[Inject(typeof(IValidationDataAccess<User>))]
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

    public async Task<ValidationResult> ValidateUpdate(User model)
    {
        var errors = new ValidationResult();
        if (await _context.Users.AsNoTracking().AnyAsync(u => u.Id != model.Id && u.Username == model.Username))
        {
            errors.AddError(new ValidationError(nameof(model.Username), "This username is already used"));
        }

        return errors;
    }

    public async Task<ValidationResult> ValidateCreate(User user)
    {
        var errors = new ValidationResult();
        if (await _context.Users.AnyAsync(u => u.Email == user.Email && u.Id != user.Id))
        {
            errors.AddError(new ValidationError(nameof(user.Email), "This email is already user"));
        }

        if (await _context.Users.AnyAsync(u => u.Username == user.Username))
        {
            errors.AddError(new ValidationError(nameof(user.Username), "This username is already user"));
        }

        return errors;
    }
}