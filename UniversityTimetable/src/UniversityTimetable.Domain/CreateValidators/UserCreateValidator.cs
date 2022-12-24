using AutoMapper;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.CreateValidators;

public class UserCreateValidator : ICreateValidator<UserDto>
{
    private readonly IMapper _mapper;
    private readonly IValidationRepository<User> _repository;
    private readonly ICreateValidator<UserDto> _baseValidator;

    public UserCreateValidator(IMapper mapper, IValidationRepository<User> repository, ICreateValidator<UserDto> baseValidator)
    {
        _mapper = mapper;
        _repository = repository;
        _baseValidator = baseValidator;
    }

    public async Task<Dictionary<string, string>> ValidateAsync(UserDto dataTransferObject)
    {
        var errors = await _baseValidator.ValidateAsync(dataTransferObject);
        var model = _mapper.Map<User>(dataTransferObject);
        errors.AddRange(await _repository.ValidateCreateOnDataBaseLevel(model));
        return errors;
    }
}