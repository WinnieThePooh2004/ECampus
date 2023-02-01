using ECampus.Contracts.DataAccess;
using ECampus.Contracts.Services;
using ECampus.Core.Metadata;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Validation;

namespace ECampus.Services.Services;

[Inject(typeof(IPasswordChangeService))]
public class PasswordChangeService : IPasswordChangeService
{
    private readonly IPasswordChangeDataAccess _passwordChangeDataAccess;
    private readonly IUpdateValidator<PasswordChangeDto> _passwordChangeValidator;

    public PasswordChangeService(IPasswordChangeDataAccess passwordChangeDataAccess,
        IUpdateValidator<PasswordChangeDto> passwordChangeValidator)
    {
        _passwordChangeDataAccess = passwordChangeDataAccess;
        _passwordChangeValidator = passwordChangeValidator;
    }

    public async Task<PasswordChangeDto> ChangePassword(PasswordChangeDto passwordChange)
    {
        var errors = await _passwordChangeValidator.ValidateAsync(passwordChange);
        if (!errors.IsValid)
        {
            throw new ValidationException(typeof(PasswordChangeDto), errors);
        }

        await _passwordChangeDataAccess.ChangePassword(passwordChange);
        return passwordChange;
    }

    public async Task<ValidationResult> ValidatePasswordChange(PasswordChangeDto passwordChange)
    {
        return await _passwordChangeValidator.ValidateAsync(passwordChange);
    }
}