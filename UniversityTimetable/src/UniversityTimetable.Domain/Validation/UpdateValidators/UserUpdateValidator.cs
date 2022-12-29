using AutoMapper;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Data.Validation;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Validation.UpdateValidators;

public class UserUpdateValidator : IUpdateValidator<UserDto>
{
    private readonly IUpdateValidator<UserDto> _baseValidator;
    private readonly IDataValidator<User> _dataAccess;
    private readonly IMapper _mapper;

    public UserUpdateValidator(IUpdateValidator<UserDto> baseValidator, IMapper mapper, IDataValidator<User> dataAccess)
    {
        _baseValidator = baseValidator;
        _mapper = mapper;
        _dataAccess = dataAccess;
    }

    public async Task<List<KeyValuePair<string, string>>> ValidateAsync(UserDto dataTransferObject)
    {
        var errors = await _baseValidator.ValidateAsync(dataTransferObject);
        var model = _mapper.Map<User>(dataTransferObject);
        errors.AddRange(await _dataAccess.ValidateUpdate(model));
        var userFromDb = await _dataAccess.LoadRequiredDataForUpdate(model);
        if (userFromDb is null)
        {
            return new List<KeyValuePair<string, string>> { KeyValuePair.Create("Id", "This user does not exist") };
        }

        if (model.Email != userFromDb.Email)
        {
            errors.Add(KeyValuePair.Create<string, string>(nameof(model.Email), "You cannot change email"));
        }

        if (model.Password != userFromDb.Password)
        {
            errors.Add(KeyValuePair.Create<string, string>(nameof(model.Password),
                "To change password use action 'Users/ChangePassword'"));
        }

        return errors;
    }
}