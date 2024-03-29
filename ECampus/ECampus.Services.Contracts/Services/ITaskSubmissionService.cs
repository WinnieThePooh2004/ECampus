﻿using ECampus.Domain.DataTransferObjects;

namespace ECampus.Services.Contracts.Services;

public interface ITaskSubmissionService
{
    public Task<TaskSubmissionDto> UpdateContentAsync(int submissionId, string content, CancellationToken token = default);
    public Task<TaskSubmissionDto> UpdateMarkAsync(int submissionId, int mark, CancellationToken token = default);
    Task<TaskSubmissionDto> GetByIdAsync(int id, CancellationToken token = default);
    Task<TaskSubmissionDto> GetByCourseAsync(int courseTaskId, CancellationToken token = default);
}