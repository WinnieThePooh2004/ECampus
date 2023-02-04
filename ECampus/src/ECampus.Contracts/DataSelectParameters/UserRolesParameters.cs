using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public class UserRolesParameters : IDataSelectParameters<User>
{
    public int UserId { get; init; }
}