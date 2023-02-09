﻿using ECampus.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class TaskSubmissionValidationDataSelect : IMultipleItemSelector<TaskSubmission, TaskSubmissionValidationParameters>
{
    public IQueryable<TaskSubmission> SelectData(ApplicationDbContext context, TaskSubmissionValidationParameters parameters)
    {
        IQueryable<TaskSubmission> source = context.TaskSubmissions;
        if (parameters.IncludeCourseTask)
        {
            source = source
                .Include(submission => submission.CourseTask);
        }

        return source.Where(submission => submission.Id == parameters.TaskSubmissionId);
    }
}