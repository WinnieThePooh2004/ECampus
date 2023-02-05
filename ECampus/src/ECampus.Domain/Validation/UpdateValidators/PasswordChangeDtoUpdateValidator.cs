using ECampus.Contracts.DataAccess;
using ECampus.Contracts.DataValidation;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;

namespace ECampus.Domain.Validation.UpdateValidators;

public class PasswordChangeDtoUpdateValidator : IUpdateValidator<PasswordChangeDto>
{
    private readonly IUpdateValidator<PasswordChangeDto> _baseValidator;
    private readonly IDataAccessManager _dataAccess;

    public PasswordChangeDtoUpdateValidator(IUpdateValidator<PasswordChangeDto> baseValidator,
        IDataAccessManagerFactory dataAccessManagerFactory)
    {
        _baseValidator = baseValidator;
        _dataAccess = dataAccessManagerFactory.Primitive;
    }

    public async Task<ValidationResult> ValidateAsync(PasswordChangeDto dataTransferObject)
    {
        var errors = await _baseValidator.ValidateAsync(dataTransferObject);
        var user = await _dataAccess.GetByIdAsync<User>(dataTransferObject.UserId);
        if (user.Password != dataTransferObject.OldPassword)
        {
            errors.AddError(new ValidationError(nameof(dataTransferObject.OldPassword), "Invalid old password"));
        }
        return errors;
    }
}