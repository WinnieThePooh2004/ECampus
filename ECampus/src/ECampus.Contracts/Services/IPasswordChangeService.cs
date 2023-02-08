using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Contracts.Services;

public interface IPasswordChangeService
{
    Task<UserDto> ChangePassword(PasswordChangeDto passwordChange, CancellationToken token = default);
    Task<ValidationResult> ValidatePasswordChange(PasswordChangeDto passwordChange, CancellationToken token = default);
}