namespace ECampus.Domain.Responses.Auditory;

public class MultipleAuditoryResponse : IMultipleItemsResponse<Entities.Auditory>
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Building { get; set; } = default!;
}