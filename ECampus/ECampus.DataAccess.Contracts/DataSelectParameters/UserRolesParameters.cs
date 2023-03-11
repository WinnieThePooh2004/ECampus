using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct UserRolesParameters : IDataSelectParameters<User>
{
    public readonly int UserId;

    public UserRolesParameters(int userId)
    {
        UserId = userId;
    }
}