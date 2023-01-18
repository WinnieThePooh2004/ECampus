using AutoMapper;
using UniversityTimetable.Shared.DataTransferObjects;
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

    public UserCreateValidator(IMapper mapper, IDataValidator<User> dataAccess, ICreateValidator<UserDto> createValidator)
    {
        _mapper = mapper;
        _dataAccess = dataAccess;
        _createValidator = createValidator;
    }

    public async Task<ValidationResult> ValidateAsync(UserDto dataTransferObject)
    {
        var errors = await _createValidator.ValidateAsync(dataTransferObject);
        var model = _mapper.Map<User>(dataTransferObject);
        errors.MergeResults(await _dataAccess.ValidateCreate(model));
        return errors;
    }
}