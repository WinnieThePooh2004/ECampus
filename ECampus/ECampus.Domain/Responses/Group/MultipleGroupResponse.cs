namespace ECampus.Domain.Responses.Group;

public class MultipleGroupResponse : IMultipleItemsResponse<Entities.Group>
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;
}