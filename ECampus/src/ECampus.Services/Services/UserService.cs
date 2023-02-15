using ECampus.Contracts.Services;
using ECampus.Core.Installers;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Services.Services;

[Inject(typeof(IUserService))]
public class UserService : IUserService
{
    private readonly ICreateValidator<UserDto> _createValidator;
    private readonly IUpdateValidator<UserDto> _updateValidator;
    private readonly IBaseService<UserDto> _baseService;

    public UserService(IUpdateValidator<UserDto> updateValidator,
        ICreateValidator<UserDto> createValidator, IBaseService<UserDto> baseService)
    {
        _updateValidator = updateValidator;
        _createValidator = createValidator;
        _baseService = baseService;
    }

    public async Task<ValidationResult> ValidateCreateAsync(UserDto user, CancellationToken token = default)
    {
        return await _createValidator.ValidateAsync(user, token);
    }

    public async Task<ValidationResult> ValidateUpdateAsync(UserDto user, CancellationToken token = default)
    {
        return await _updateValidator.ValidateAsync(user, token);
    }

    public Task<UserDto> GetByIdAsync(int id, CancellationToken token = default) =>
        _baseService.GetByIdAsync(id, token);

    public Task<UserDto> CreateAsync(UserDto entity, CancellationToken token = default) =>
        _baseService.CreateAsync(entity, token);

    public Task<UserDto> UpdateAsync(UserDto entity, CancellationToken token = default) =>
        _baseService.UpdateAsync(entity, token);

    public Task<UserDto> DeleteAsync(int id, CancellationToken token = default) => _baseService.DeleteAsync(id, token);
}