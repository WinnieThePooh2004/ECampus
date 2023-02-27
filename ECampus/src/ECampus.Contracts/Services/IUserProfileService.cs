﻿using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Contracts.Services;

public interface IUserProfileService
{
    Task<ValidationResult> ValidateCreateAsync(UserDto user, CancellationToken token = default);
    Task<ValidationResult> ValidateUpdateAsync(UserDto user, CancellationToken token = default);
    Task<UserProfile> UpdateProfileAsync(UserProfile user, CancellationToken token);
    Task<UserProfile> GetByIdAsync(int id, CancellationToken token);
}