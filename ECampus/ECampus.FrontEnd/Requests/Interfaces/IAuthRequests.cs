using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Responses.Auth;

namespace ECampus.FrontEnd.Requests.Interfaces;

public interface IAuthRequests
{
    Task<LoginResponse> LoginAsync(LoginDto login);
    Task<LoginResponse> SignUpAsync(RegistrationDto registrationDto);
}