namespace ECampus.Domain.Responses.Faculty;

public class MultipleFacultyResponse : IMultipleItemsResponse<Entities.Faculty>
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;
}