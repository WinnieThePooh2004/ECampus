namespace ECampus.Domain.Interfaces.Auth;

public interface IAuthenticationService
{
    void VerifyUser(int userId);
}