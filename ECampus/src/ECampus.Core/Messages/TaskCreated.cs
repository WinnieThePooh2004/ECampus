namespace ECampus.Core.Messages;

public class TaskCreated : ISqsMessage
{
    public required string TaskName { get; set; }
    public required string CourseName { get; set; }
    public required DateTime Deadline { get; set; }
    public required string TaskType { get; set; }
    public required int MaxPoints { get; set; }
    public required List<string?> StudentEmails { get; init; }
}