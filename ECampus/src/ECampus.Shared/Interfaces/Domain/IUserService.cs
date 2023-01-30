﻿using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Shared.Interfaces.Domain;

public interface IUserService
{
    Task<ValidationResult> ValidateCreateAsync(UserDto user);
    Task<ValidationResult> ValidateUpdateAsync(UserDto user);
}