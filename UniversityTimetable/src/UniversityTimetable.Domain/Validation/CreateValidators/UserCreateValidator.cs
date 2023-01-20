﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Validation;

namespace UniversityTimetable.Domain.Validation.CreateValidators;

public class UserCreateValidator : ICreateValidator<UserDto>
{
    private readonly IMapper _mapper;
    private readonly IDataValidator<User> _dataAccess;
    private readonly ICreateValidator<UserDto> _createValidator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserCreateValidator(IMapper mapper, IDataValidator<User> dataAccess,
        ICreateValidator<UserDto> createValidator, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _dataAccess = dataAccess;
        _createValidator = createValidator;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ValidationResult> ValidateAsync(UserDto dataTransferObject)
    {
        var errors = await _createValidator.ValidateAsync(dataTransferObject);
        ValidateRole(dataTransferObject, errors);
        var model = _mapper.Map<User>(dataTransferObject);
        errors.MergeResults(await _dataAccess.ValidateCreate(model));
        return errors;
    }

    private void ValidateRole(UserDto user, ValidationResult currentErrors)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new HttpContextNotFoundExceptions();
        }

        if (user.Role == UserRole.Guest || _httpContextAccessor.HttpContext.User.IsInRole(nameof(UserRole.Admin)))
        {
            return;
        }

        currentErrors.AddError(new ValidationError(nameof(user.Role),
            $"Only admin can create user with roles different from {nameof(UserRole.Guest)}"));
    }
}