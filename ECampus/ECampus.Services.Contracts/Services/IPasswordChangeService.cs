using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Validation;

namespace ECampus.Services.Contracts.Services;

public interface IPasswordChangeService
{
    Task<UserDto> ChangePassword(PasswordChangeDto passwordChange, CancellationToken token = default);
    Task<ValidationResult> ValidatePasswordChange(PasswordChangeDto passwordChange, CancellationToken token = default);
}