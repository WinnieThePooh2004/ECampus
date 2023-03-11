using AutoMapper;
using ECampus.Core.Installers;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Exceptions.DomainExceptions;
using ECampus.Domain.Validation;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Contracts.Validation;

namespace ECampus.Services.Services;

[Inject(typeof(IPasswordChangeService))]
public class PasswordChangeService : IPasswordChangeService
{
    private readonly IDataAccessFacade _dataAccess;
    private readonly IUpdateValidator<PasswordChangeDto> _passwordChangeValidator;
    private readonly IMapper _mapper;

    public PasswordChangeService(IUpdateValidator<PasswordChangeDto> passwordChangeValidator, IMapper mapper,
        IDataAccessFacade dataAccessFacadeFactory)
    {
        _passwordChangeValidator = passwordChangeValidator;
        _mapper = mapper;
        _dataAccess = dataAccessFacadeFactory;
    }

    public async Task<UserDto> ChangePassword(PasswordChangeDto passwordChange, CancellationToken token = default)
    {
        var errors = await _passwordChangeValidator.ValidateAsync(passwordChange, token);
        if (!errors.IsValid)
        {
            throw new ValidationException(typeof(PasswordChangeDto), errors);
        }

        var user = await _dataAccess.PureByIdAsync<User>(passwordChange.UserId, token);
        user.Password = passwordChange.NewPassword;
        await _dataAccess.SaveChangesAsync(token);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<ValidationResult> ValidatePasswordChange(PasswordChangeDto passwordChange, CancellationToken token = default)
    {
        return await _passwordChangeValidator.ValidateAsync(passwordChange, token);
    }
}