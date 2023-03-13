namespace ECampus.Domain.Responses.Subject;

public class MultipleSubjectResponse : IMultipleItemsResponse<Entities.Subject>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
}