namespace ECampus.Shared.Messaging.TaskSubmissions;

public class SubmissionMarked : IMessage
{
    public required string? UserEmail { get; init; }
    public required string? TaskName { get; init; }
    public required int MaxPoints { get; set; }
    public required int ScoredPoints { get; set; }
}