using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Shared.Interfaces.Domain;

public interface IPasswordChangeService
{
    Task<PasswordChangeDto> ChangePassword(PasswordChangeDto passwordChange);
    Task<ValidationResult> ValidatePasswordChange(PasswordChangeDto passwordChange);
}