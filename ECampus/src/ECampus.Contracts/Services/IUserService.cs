using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Contracts.Services;

public interface IUserService
{
    Task<ValidationResult> ValidateCreateAsync(UserDto user);
    Task<ValidationResult> ValidateUpdateAsync(UserDto user);
}