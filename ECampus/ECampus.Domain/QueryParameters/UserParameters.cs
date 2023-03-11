using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;

namespace ECampus.Domain.QueryParameters;

public class UserParameters : QueryParameters<UserDto>, IDataSelectParameters<User>
{
    public string? Email { get; set; }
    public string? Username { get; set; }
}