﻿namespace ECampus.Core.Messages;

public class SubmissionEdited : ISqsMessage
{
    public required string? UserEmail { get; init; }
    public required string TaskName { get; init; }
}