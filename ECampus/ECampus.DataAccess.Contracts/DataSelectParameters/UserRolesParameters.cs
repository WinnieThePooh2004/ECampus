using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct UserRolesParameters : IDataSelectParameters<User>
{
    public readonly int UserId;

    public UserRolesParameters(int userId)
    {
        UserId = userId;
    }
}