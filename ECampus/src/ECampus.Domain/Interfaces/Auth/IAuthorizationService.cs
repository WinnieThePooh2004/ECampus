using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Domain.Interfaces.Auth;

public interface IAuthorizationService
{
    Task<LoginResult> Login(LoginDto login, CancellationToken token = default);
    Task<LoginResult> SignUp(RegistrationDto registrationDto, CancellationToken token = default);
    Task<ValidationResult> ValidateSignUp(RegistrationDto registrationDto, CancellationToken token = default);
    Task<ValidationResult> ValidateLogin(LoginDto login, CancellationToken token = default);
}