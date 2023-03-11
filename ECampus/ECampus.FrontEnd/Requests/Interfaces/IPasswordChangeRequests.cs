using ECampus.Domain.DataTransferObjects;

namespace ECampus.FrontEnd.Requests.Interfaces;

public interface IPasswordChangeRequests
{
    Task ChangePassword(PasswordChangeDto passwordChange);
}