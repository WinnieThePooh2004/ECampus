using ECampus.Domain.Entities;
using ECampus.Domain.Requests;

namespace ECampus.DataAccess.Contracts.DataSelectParameters;

public readonly struct UserRolesParameters : IDataSelectParameters<User>
{
    public readonly int UserId;

    public UserRolesParameters(int userId)
    {
        UserId = userId;
    }
}