using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.Services;
using ECampus.Core.Installers;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;

namespace ECampus.Services.Services;

[Inject(typeof(IPasswordChangeService))]
public class PasswordChangeService : IPasswordChangeService
{
    private readonly IDataAccessManager _dataAccess;
    private readonly IUpdateValidator<PasswordChangeDto> _passwordChangeValidator;
    private readonly IMapper _mapper;

    public PasswordChangeService(IUpdateValidator<PasswordChangeDto> passwordChangeValidator, IMapper mapper,
        IDataAccessManagerFactory dataAccessManagerFactory)
    {
        _passwordChangeValidator = passwordChangeValidator;
        _mapper = mapper;
        _dataAccess = dataAccessManagerFactory.Primitive;
    }

    public async Task<UserDto> ChangePassword(PasswordChangeDto passwordChange, CancellationToken token = default)
    {
        var errors = await _passwordChangeValidator.ValidateAsync(passwordChange, token);
        if (!errors.IsValid)
        {
            throw new ValidationException(typeof(PasswordChangeDto), errors);
        }

        var user = await _dataAccess.GetByIdAsync<User>(passwordChange.UserId, token);
        user.Password = passwordChange.NewPassword;
        await _dataAccess.SaveChangesAsync(token);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<ValidationResult> ValidatePasswordChange(PasswordChangeDto passwordChange, CancellationToken token = default)
    {
        return await _passwordChangeValidator.ValidateAsync(passwordChange, token);
    }
}