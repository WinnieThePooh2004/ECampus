using ECampus.Domain.Responses.User;

namespace ECampus.Domain.Requests.User;

public class UserParameters : QueryParameters<MultipleUserResponse>, IDataSelectParameters<Entities.User>
{
    public string? Email { get; set; }
    public string? Username { get; set; }
}