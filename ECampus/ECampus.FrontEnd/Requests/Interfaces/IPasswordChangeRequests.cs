using ECampus.Shared.DataTransferObjects;

namespace ECampus.FrontEnd.Requests.Interfaces;

public interface IPasswordChangeRequests
{
    Task ChangePassword(PasswordChangeDto passwordChange);
}