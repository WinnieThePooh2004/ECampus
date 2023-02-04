using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class UserParameters : QueryParameters, IDataSelectParameters<User>
{
    public string? Email { get; set; }
    public string? Username { get; set; }
}