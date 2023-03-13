using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Responses.Auth;
using ECampus.Domain.Validation;

namespace ECampus.Services.Contracts.Services;

public interface IAuthorizationService
{
    Task<LoginResponse> Login(LoginDto login, CancellationToken token = default);
    Task<LoginResponse> SignUp(RegistrationDto registrationDto, CancellationToken token = default);
    Task<ValidationResult> ValidateSignUp(RegistrationDto registrationDto, CancellationToken token = default);
    Task<ValidationResult> ValidateLogin(LoginDto login, CancellationToken token = default);
}