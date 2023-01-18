using AutoMapper;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Validation;

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

    public async Task<ValidationResult> ValidateAsync(UserDto dataTransferObject)
    {
        var errors = await _updateValidator.ValidateAsync(dataTransferObject);
        var model = _mapper.Map<User>(dataTransferObject);
        errors.MergeResults(await _dataAccess.ValidateUpdate(model));
        var userFromDb = await _validationDataAccess.LoadRequiredDataForUpdateAsync(model);

        if (model.Email != userFromDb.Email)
        {
            errors.AddError(new ValidationError(nameof(model.Email), "You cannot change email"));
        }

        if (model.Password != userFromDb.Password)
        {
            errors.AddError(new ValidationError(nameof(model.Password),
                "To change password use action 'Users/ChangePassword'"));
        }

        return errors;
    }
}