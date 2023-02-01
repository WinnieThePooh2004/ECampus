using AutoMapper;
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
    private readonly IMapper _mapper;

    public PasswordChangeService(IPasswordChangeDataAccess passwordChangeDataAccess,
        IUpdateValidator<PasswordChangeDto> passwordChangeValidator, IMapper mapper)
    {
        _passwordChangeDataAccess = passwordChangeDataAccess;
        _passwordChangeValidator = passwordChangeValidator;
        _mapper = mapper;
    }

    public async Task<UserDto> ChangePassword(PasswordChangeDto passwordChange)
    {
        var errors = await _passwordChangeValidator.ValidateAsync(passwordChange);
        if (!errors.IsValid)
        {
            throw new ValidationException(typeof(PasswordChangeDto), errors);
        }

        var user = await _passwordChangeDataAccess.ChangePassword(passwordChange);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<ValidationResult> ValidatePasswordChange(PasswordChangeDto passwordChange)
    {
        return await _passwordChangeValidator.ValidateAsync(passwordChange);
    }
}