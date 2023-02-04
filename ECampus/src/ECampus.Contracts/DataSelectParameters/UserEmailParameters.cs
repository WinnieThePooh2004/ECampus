using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public class UserEmailParameters : IDataSelectParameters<User>
{
    public string Email { get; set; } = default!;
}