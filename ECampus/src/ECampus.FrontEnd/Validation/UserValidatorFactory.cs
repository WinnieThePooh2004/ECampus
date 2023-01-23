﻿using ECampus.FrontEnd.Requests.Interfaces.Validation;
using ECampus.FrontEnd.Validation.Interfaces;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Metadata;
using FluentValidation;

namespace ECampus.FrontEnd.Validation;

[Inject(typeof(IUserValidatorFactory))]
public class UserValidatorFactory : IUserValidatorFactory
{
    private readonly IUpdateValidationRequests<UserDto> _updateValidationRequests;
    private readonly ICreateValidationRequests<UserDto> _createValidationRequests;
    private readonly IValidator<UserDto> _baseValidator;

    public UserValidatorFactory(IValidator<UserDto> baseValidator,
        ICreateValidationRequests<UserDto> createValidationRequests,
        IUpdateValidationRequests<UserDto> updateValidationRequests)
    {
        _baseValidator = baseValidator;
        _createValidationRequests = createValidationRequests;
        _updateValidationRequests = updateValidationRequests;
    }

    public IValidator<UserDto> CreateValidator()
        => new HttpCallingValidator<UserDto>(_baseValidator, _createValidationRequests);

    public IValidator<UserDto> UpdateValidator()
        => new HttpCallingValidator<UserDto>(_baseValidator, _updateValidationRequests);
}