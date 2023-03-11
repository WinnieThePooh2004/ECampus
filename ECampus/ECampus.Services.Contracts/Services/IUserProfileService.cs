using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Validation;

namespace ECampus.Services.Contracts.Services;

public interface IUserProfileService
{
    Task<ValidationResult> ValidateCreateAsync(UserDto user, CancellationToken token = default);
    Task<ValidationResult> ValidateUpdateAsync(UserDto user, CancellationToken token = default);
    Task<UserProfile> UpdateProfileAsync(UserProfile user, CancellationToken token);
    Task<UserProfile> GetByIdAsync(int id, CancellationToken token);
}