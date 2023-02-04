using ECampus.Contracts.Services;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;

namespace ECampus.Services.Services.ValidationServices;

public class UserRolesValidationService : IUserRolesService
{
    private readonly IUserRolesService _baseService;
    private readonly ICreateValidator<UserDto> _updateValidator;
    private readonly ICreateValidator<UserDto> _createValidator;

    public UserRolesValidationService(IUserRolesService baseService, ICreateValidator<UserDto> updateValidator,
        ICreateValidator<UserDto> createValidator)
    {
        _baseService = baseService;
        _updateValidator = updateValidator;
        _createValidator = createValidator;
    }

    public Task<UserDto> GetByIdAsync(int id)
    {
        return _baseService.GetByIdAsync(id);
    }

    public async Task<UserDto> UpdateAsync(UserDto user)
    {
        var validationResult = await _updateValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(typeof(UserDto), validationResult);
        }

        return await _baseService.UpdateAsync(user);
    }

    public async Task<UserDto> CreateAsync(UserDto user)
    {
        var validationResult = await _createValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(typeof(UserDto), validationResult);
        }

        return await _baseService.UpdateAsync(user);
    }

    public Task<UserDto> DeleteAsync(int id) => _baseService.DeleteAsync(id);
}