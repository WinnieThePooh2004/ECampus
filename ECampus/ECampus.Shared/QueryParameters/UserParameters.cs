using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class UserParameters : QueryParameters<UserDto>, IDataSelectParameters<User>
{
    public string? Email { get; set; }
    public string? Username { get; set; }
}