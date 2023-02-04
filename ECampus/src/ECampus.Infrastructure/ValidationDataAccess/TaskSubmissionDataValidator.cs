﻿using ECampus.Contracts.DataValidation;
using ECampus.Core.Metadata;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.ValidationDataAccess;

[Inject(typeof(ITaskSubmissionDataValidator))]
public class TaskSubmissionDataValidator : ITaskSubmissionDataValidator
{
    private readonly ApplicationDbContext _context;

    public TaskSubmissionDataValidator(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TaskSubmission> LoadSubmissionData(int taskSubmissionId)
    {
        return await _context.TaskSubmissions
                   .Include(t => t.CourseTask)
                   .FirstOrDefaultAsync(t => t.Id == taskSubmissionId) ??
               throw new ObjectNotFoundByIdException(typeof(TaskSubmission), taskSubmissionId);
    }

    public async Task<ValidationResult> ValidateTeacherId(int teacherId, int taskSubmissionId)
    {
        var result = new ValidationResult();
        var submissionAuthorGroup = await FindSubmissionAuthorGroup(taskSubmissionId);

        await ValidateTeacherTeachesAuthorsGroup(teacherId, submissionAuthorGroup.Id, result);

        return result;
    }

    private async Task<Group> FindSubmissionAuthorGroup(int taskSubmissionId)
    {
        var result = await _context.TaskSubmissions
                         .Include(submission => submission.Student)
                         .ThenInclude(student => student!.Group)
                         .SingleOrDefaultAsync(submission => submission.Id == taskSubmissionId) ??
                     throw new ObjectNotFoundByIdException(typeof(TaskSubmission), taskSubmissionId);

        return result.Student!.Group!;
    }

    private async Task ValidateTeacherTeachesAuthorsGroup(int teacherId, int submissionAuthorGroupId,
        ValidationResult result)
    {
        if (!await _context.Teachers
                .Include(t => t.Courses)!
                .ThenInclude(c => c.Groups)
                .AnyAsync(t => t.Courses!.Any(c => c.Groups!.Any(g => g.Id == submissionAuthorGroupId))))
        {
            result.AddError(new ValidationError(nameof(teacherId),
                $"Current user is logged in as teacher with id = {teacherId}," +
                " but this teacher does not teaches submission author`s group"));
        }
    }
}