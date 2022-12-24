using AutoMapper;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.UpdateValidators;

public class UserUpdateValidator : IUpdateValidator<UserDto>
{
    private readonly IUpdateValidator<UserDto> _baseValidator;
    private readonly IValidationRepository<User> _repository;
    private readonly IMapper _mapper;

    public UserUpdateValidator(IUpdateValidator<UserDto> baseValidator, IMapper mapper, IValidationRepository<User> repository)
    {
        _baseValidator = baseValidator;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Dictionary<string, string>> ValidateAsync(UserDto dataTransferObject)
    {
        var errors = await _baseValidator.ValidateAsync(dataTransferObject);
        var model = _mapper.Map<User>(dataTransferObject);
        errors.AddRange(await _repository.ValidateUpdateOnDataBaseLevel(model));
        var userFromDb = await _repository.LoadRequiredDataForUpdate(model);
        if (userFromDb is null)
        {
            return new Dictionary<string, string> { ["Id"] = "This user does not exist" };
        }
        if (model.Email != userFromDb.Email)
        {
            errors.Add(nameof(model.Email), "You cannot change email");
        }
        if (model.Password != userFromDb.Password)
        {
            errors.Add(nameof(model.Password), "To change password use action 'Users/ChangePassword'");
        }
        return errors;
    }
}