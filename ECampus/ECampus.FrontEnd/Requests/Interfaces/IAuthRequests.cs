using ECampus.Shared.DataTransferObjects;

namespace ECampus.FrontEnd.Requests.Interfaces;

public interface IAuthRequests
{
    Task<LoginResult> LoginAsync(LoginDto login);
    Task<LoginResult> SignUpAsync(RegistrationDto registrationDto);
}