using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public class UserUsernameParameters : IDataSelectParameters<User>
{
    public string Username { get; set; } = default!;
}