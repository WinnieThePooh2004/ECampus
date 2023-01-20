using AutoMapper;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;
using UniversityTimetable.Shared.Metadata;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Services;

[Inject(typeof(IUserRolesService))]
public class UserRolesService : IUserRolesService
{
    private readonly IUserRolesDataAccessFacade _dataAccess;
    private readonly IMapper _mapper;
    private readonly IUpdateValidator<UserDto> _updateValidator;
    private readonly ICreateValidator<UserDto> _createValidator;

    public UserRolesService(IUserRolesDataAccessFacade dataAccess, IMapper mapper,
        IUpdateValidator<UserDto> updateValidator, ICreateValidator<UserDto> createValidator)
    {
        _dataAccess = dataAccess;
        _mapper = mapper;
        _updateValidator = updateValidator;
        _createValidator = createValidator;
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        return _mapper.Map<UserDto>(await _dataAccess.GetByIdAsync(id));
    }

    public async Task<UserDto> UpdateAsync(UserDto user)
    {
        var errors = await _updateValidator.ValidateAsync(user);
        if (!errors.IsValid)
        {
            throw new ValidationException(typeof(UserDto), errors);
        }

        return _mapper.Map<UserDto>(await _dataAccess.UpdateAsync(_mapper.Map<User>(user)));
    }
    
    public async Task<UserDto> CreateAsync(UserDto user)
    {
        var errors = await _createValidator.ValidateAsync(user);
        if (!errors.IsValid)
        {
            throw new ValidationException(typeof(UserDto), errors);
        }

        return _mapper.Map<UserDto>(await _dataAccess.CreateAsync(_mapper.Map<User>(user)));
    }
}