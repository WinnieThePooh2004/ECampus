using ECampus.Contracts.DataAccess;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;

namespace ECampus.Domain.Validation.UniversalValidators;

public abstract class UserValidatorBase
{
    private readonly IDataAccessManager _dataAccess;

    protected UserValidatorBase(IDataAccessManager dataAccess)
    {
        _dataAccess = dataAccess;
    }

    protected async Task<ValidationResult> ValidateRole(UserDto user, CancellationToken token) =>
        user.Role switch
        {
            UserRole.Admin or UserRole.Guest => new ValidationResult(),
            UserRole.Student => await ValidateAsStudent(user, token),
            UserRole.Teacher => await ValidateAsTeacher(user, token),
            _ => new ValidationResult(nameof(user.Role), $"No such role found '{user.Role}'")
        };

    private async Task<ValidationResult> ValidateAsTeacher(UserDto user, CancellationToken token)
    {
        var teacher = await _dataAccess.PureOrDefaultByIdAsync<Teacher>((int)user.TeacherId!, token);
        if (teacher is null)
        {
            return new ValidationResult(new ValidationError(nameof(user.TeacherId),
                $"Teacher with id {user.TeacherId} does not exist"));
        }

        if (teacher.UserEmail is not null && teacher.UserEmail != user.Email)
        {
            return new ValidationResult(nameof(user.TeacherId),
                $"Cannot bind to teacher with id {user.TeacherId} because it is " +
                $"already bind to user with email {teacher.UserEmail}");
        }

        return new ValidationResult();
    }

    private async Task<ValidationResult> ValidateAsStudent(UserDto user, CancellationToken token)
    {
        var student = await _dataAccess.PureOrDefaultByIdAsync<Student>((int)user.StudentId!, token);
        if (student is null)
        {
            return new ValidationResult(new ValidationError(nameof(user.StudentId),
                $"Student with id {user.StudentId} does not exist"));
        }

        if (student.UserEmail is not null  && student.UserEmail != user.Email)
        {
            return new ValidationResult(nameof(user.StudentId),
                $"Cannot bind to student with id {user.StudentId} because it is " +
                $"already bind to user with email {student.UserEmail}");
        }

        return new ValidationResult();
    }
}