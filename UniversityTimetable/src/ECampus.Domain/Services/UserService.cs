using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Metadata;
using ECampus.Shared.Validation;

namespace ECampus.Domain.Services;

[Inject(typeof(IUserService))]
public class UserService : IUserService
{
    private readonly IUserDataAccessFacade _userDataAccessFacade;
    private readonly ICreateValidator<UserDto> _createValidator;
    private readonly IUpdateValidator<UserDto> _updateValidator;
    private readonly IUpdateValidator<PasswordChangeDto> _passwordChangeValidator;

    public UserService(IUpdateValidator<PasswordChangeDto> passwordChangeValidator,
        IUserDataAccessFacade userDataAccessFacade, IUpdateValidator<UserDto> updateValidator,
        ICreateValidator<UserDto> createValidator)
    {
        _passwordChangeValidator = passwordChangeValidator;
        _userDataAccessFacade = userDataAccessFacade;
        _updateValidator = updateValidator;
        _createValidator = createValidator;
    }

    public async Task<ValidationResult> ValidateCreateAsync(UserDto user)
    {
        return await _createValidator.ValidateAsync(user);
    }

    public async Task<ValidationResult> ValidateUpdateAsync(UserDto user)
    {
        return await _updateValidator.ValidateAsync(user);
    }

    public async Task<PasswordChangeDto> ChangePassword(PasswordChangeDto passwordChange)
    {
        var errors = await _passwordChangeValidator.ValidateAsync(passwordChange);
        if (!errors.IsValid)
        {
            throw new ValidationException(typeof(PasswordChangeDto), errors);
        }

        await _userDataAccessFacade.ChangePassword(passwordChange);
        return passwordChange;
    }

    public async Task<ValidationResult> ValidatePasswordChange(PasswordChangeDto passwordChange)
    {
        return await _passwordChangeValidator.ValidateAsync(passwordChange);
    }
}