using ECampus.Shared.Enums;
using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class UserParameters : QueryParameters, IQueryParameters<User>
{
    public string? Email { get; set; }
    public UserRole Role { get; set; } = UserRole.Guest;
    public string? Username { get; set; }
}