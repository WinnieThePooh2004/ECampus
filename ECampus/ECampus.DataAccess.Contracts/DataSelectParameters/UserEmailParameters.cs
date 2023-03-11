using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct UserEmailParameters : IDataSelectParameters<User>
{
    public readonly string Email;

    public UserEmailParameters(string email)
    {
        Email = email;
    }
}