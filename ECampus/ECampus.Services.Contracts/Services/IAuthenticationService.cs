namespace ECampus.Services.Contracts.Services;

public interface IAuthenticationService
{
    /// <summary>
    /// verify that provided id is current user id, if not, throws exception
    /// </summary>
    /// <param name="userId"></param>
    void VerifyUser(int userId);
}