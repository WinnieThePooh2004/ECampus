using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Contracts.Services;

public interface IUserService
{
    Task<ValidationResult> ValidateCreateAsync(UserDto user, CancellationToken token = default);
    Task<ValidationResult> ValidateUpdateAsync(UserDto user, CancellationToken token = default);
}