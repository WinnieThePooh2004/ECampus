namespace ECampus.Shared.Messaging.TaskSubmissions;

public class SubmissionEdited : IMessage
{
    public required string? UserEmail { get; init; }
    public required string TaskName { get; init; }
}