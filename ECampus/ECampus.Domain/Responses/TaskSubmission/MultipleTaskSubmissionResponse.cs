using ECampus.Domain.Metadata;

namespace ECampus.Domain.Responses.TaskSubmission;

public class MultipleTaskSubmissionResponse : IMultipleItemsResponse<Entities.TaskSubmission>
{
    public int Id { get; set; }
    
    public bool IsMarked { get; set; }
    
    [NotDisplay] public string SubmissionContent { get; set; } = string.Empty;
    
    [DisplayName("Total points", int.MaxValue - 1)] public int TotalPoints { get; set; }
    
    public int MaxPoints { get; set; }
    
    public string? StudentEmail { get; set; }

    public string StudentName { get; set; } = default!;
}