using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.Services;
using ECampus.Core.Installers;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;

namespace ECampus.Services.Services;

[Inject(typeof(IUserService))]
public class UserService : IUserService
{
    private readonly ICreateValidator<UserDto> _createValidator;
    private readonly IUpdateValidator<UserDto> _updateValidator;
    private readonly IDataAccessManager _dataAccess;
    private readonly IMapper _mapper;

    public UserService(IUpdateValidator<UserDto> updateValidator, ICreateValidator<UserDto> createValidator,
        IMapper mapper, IDataAccessManager dataAccess)
    {
        _updateValidator = updateValidator;
        _createValidator = createValidator;
        _mapper = mapper;
        _dataAccess = dataAccess;
    }

    public async Task<ValidationResult> ValidateCreateAsync(UserDto user, CancellationToken token = default)
    {
        return await _createValidator.ValidateAsync(user, token);
    }

    public async Task<ValidationResult> ValidateUpdateAsync(UserDto user, CancellationToken token = default)
    {
        return await _updateValidator.ValidateAsync(user, token);
    }

    public async Task<UserProfile> UpdateProfileAsync(UserProfile user, CancellationToken token)
    {
        var entity = _mapper.Map<User>(user);
        var updated = await _dataAccess.UpdateAsync(entity, token);
        return _mapper.Map<UserProfile>(updated);
    }

    public async Task<UserProfile> GetByIdAsync(int id, CancellationToken token = default)
    {
        var user = await _dataAccess.GetByIdAsync<User>(id, token);
        return _mapper.Map<UserProfile>(user);
    }
}