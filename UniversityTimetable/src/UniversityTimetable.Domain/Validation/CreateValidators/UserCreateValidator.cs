using AutoMapper;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;
using UniversityTimetable.Shared.Models;

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

    public async Task<List<KeyValuePair<string, string>>> ValidateAsync(UserDto dataTransferObject)
    {
        var errors = await _createValidator.ValidateAsync(dataTransferObject);
        var model = _mapper.Map<User>(dataTransferObject);
        errors.AddRange(await _dataAccess.ValidateCreate(model));
        return errors;
    }
}