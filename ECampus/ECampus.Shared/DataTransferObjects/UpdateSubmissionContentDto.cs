﻿namespace ECampus.Shared.DataTransferObjects;

public class UpdateSubmissionContentDto
{
    public int SubmissionId { get; set; }
    public string Content { get; set; } = default!;
}