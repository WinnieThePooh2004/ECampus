using FluentValidation;
using UniversityTimetable.Domain.Validation;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.FrontEnd.Validation.Interfaces;

namespace UniversityTimetable.FrontEnd.Validation;

public class UserValidatorFactory : IUserValidatorFactory
{
    private readonly IUserRequests _requests;
    private readonly IValidator<UserDto> _baseCreateValidator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserValidatorFactory(IUserRequests requests, IValidator<UserDto> baseCreateValidator, IHttpContextAccessor httpContextAccessor)
    {
        _requests = requests;
        _baseCreateValidator = baseCreateValidator;
        _httpContextAccessor = httpContextAccessor;
    }

    public IValidator<UserDto> CreateValidator() 
        => new CreateUserValidator(_baseCreateValidator, _requests, _httpContextAccessor);

    public IValidator<UserDto> UpdateValidator() 
        => new UpdateUserValidator(_requests);
}