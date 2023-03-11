using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct UserUsernameParameters : IDataSelectParameters<User>
{
    public readonly string Username;

    public UserUsernameParameters(string username)
    {
        Username = username;
    }
}