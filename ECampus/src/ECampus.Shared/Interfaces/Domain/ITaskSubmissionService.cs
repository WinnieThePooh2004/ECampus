﻿using ECampus.Shared.DataTransferObjects;

namespace ECampus.Shared.Interfaces.Domain;

public interface ITaskSubmissionService
{
    public Task UpdateContentAsync(int submissionId, string content);
    public Task UpdateMarkAsync(int submissionId, int mark);
    Task<TaskSubmissionDto> GetByIdAsync(int id);
    Task<TaskSubmissionDto> GetByCourse(int courseTaskId);
}