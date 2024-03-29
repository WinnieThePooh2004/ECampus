﻿using ECampus.Domain.Entities;
using ECampus.Domain.Requests;

namespace ECampus.DataAccess.Contracts.DataSelectParameters;

public readonly struct TaskSubmissionValidationParameters : IDataSelectParameters<TaskSubmission>
{
    public bool IncludeCourseTask { get; init; }
    public required int TaskSubmissionId { get; init; }
}