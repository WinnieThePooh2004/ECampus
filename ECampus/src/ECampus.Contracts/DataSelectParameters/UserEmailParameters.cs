using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct UserEmailParameters : IDataSelectParameters<User>
{
    public readonly string Email;

    public UserEmailParameters(string email)
    {
        Email = email;
    }
}