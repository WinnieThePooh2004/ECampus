using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Validation;

namespace UniversityTimetable.Domain.Validation.UpdateValidators;

public class PasswordChangeDtoUpdateValidator : IUpdateValidator<PasswordChangeDto>
{
    private readonly IUpdateValidator<PasswordChangeDto> _baseValidator;
    private readonly IValidationDataAccess<User> _dataAccess;

    public PasswordChangeDtoUpdateValidator(IUpdateValidator<PasswordChangeDto> baseValidator,
        IValidationDataAccess<User> dataAccess)
    {
        _baseValidator = baseValidator;
        _dataAccess = dataAccess;
    }

    public async Task<ValidationResult> ValidateAsync(PasswordChangeDto dataTransferObject)
    {
        var errors = await _baseValidator.ValidateAsync(dataTransferObject);
        var user = await _dataAccess.LoadRequiredDataForUpdateAsync(new User { Id = dataTransferObject.UserId });
        if (user.Password != dataTransferObject.OldPassword)
        {
            errors.AddError(new ValidationError(nameof(dataTransferObject.OldPassword), "Invalid old password"));
        }
        return errors;
    }
}