using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class UserParameters : QueryParameters, IQueryParameters<User>
{
    public string? Email { get; set; }
    public string? Username { get; set; }
}