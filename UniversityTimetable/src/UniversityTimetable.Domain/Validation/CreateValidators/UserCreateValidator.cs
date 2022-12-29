using AutoMapper;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Data.Validation;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Validation.CreateValidators;

public class UserCreateValidator : ICreateValidator<UserDto>
{
    private readonly IMapper _mapper;
    private readonly IDataValidator<User> _dataAccess;
    private readonly ICreateValidator<UserDto> _baseValidator;

    public UserCreateValidator(IMapper mapper, IDataValidator<User> dataAccess, ICreateValidator<UserDto> baseValidator)
    {
        _mapper = mapper;
        _dataAccess = dataAccess;
        _baseValidator = baseValidator;
    }

    public async Task<List<KeyValuePair<string, string>>> ValidateAsync(UserDto dataTransferObject)
    {
        var errors = await _baseValidator.ValidateAsync(dataTransferObject);
        var model = _mapper.Map<User>(dataTransferObject);
        errors.AddRange(await _dataAccess.ValidateCreate(model));
        return errors;
    }
}