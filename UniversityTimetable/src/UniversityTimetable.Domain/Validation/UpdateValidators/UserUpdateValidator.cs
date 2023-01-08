using AutoMapper;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Validation.UpdateValidators;

public class UserUpdateValidator : IUpdateValidator<UserDto>
{
    private readonly IUpdateValidator<UserDto> _updateValidator;
    private readonly IDataValidator<User> _dataAccess;
    private readonly IValidationDataAccess<User> _validationDataAccess;
    private readonly IMapper _mapper;

    public UserUpdateValidator(IUpdateValidator<UserDto> updateValidator, IMapper mapper,
        IDataValidator<User> dataAccess, IValidationDataAccess<User> validationDataAccess)
    {
        _updateValidator = updateValidator;
        _mapper = mapper;
        _dataAccess = dataAccess;
        _validationDataAccess = validationDataAccess;
    }

    public async Task<List<KeyValuePair<string, string>>> ValidateAsync(UserDto dataTransferObject)
    {
        var errors = await _updateValidator.ValidateAsync(dataTransferObject);
        var model = _mapper.Map<User>(dataTransferObject);
        errors.AddRange(await _dataAccess.ValidateUpdate(model));
        var userFromDb = await _validationDataAccess.LoadRequiredDataForUpdateAsync(model);

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