namespace ECampus.Domain.Responses.User;

public class MultipleUserResponse : IMultipleItemsResponse<Entities.User>
{
    public int Id { get; set; }
}