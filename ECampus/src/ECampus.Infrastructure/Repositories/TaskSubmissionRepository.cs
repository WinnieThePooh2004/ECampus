﻿using System.Net;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.Repositories;

[Inject(typeof(ITaskSubmissionRepository))]
public class TaskSubmissionRepository : ITaskSubmissionRepository
{
    private readonly ApplicationDbContext _context;

    public TaskSubmissionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task UpdateContent(int submissionId, string content)
    {
        var submission = await _context.TaskSubmissions.FindAsync(submissionId) ??
                         throw new ObjectNotFoundByIdException(typeof(TaskSubmission), submissionId);

        submission.SubmissionContent = content;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateMark(int submissionId, int mark)
    {
        var submission = await _context.TaskSubmissions.FindAsync(submissionId) ??
                         throw new ObjectNotFoundByIdException(typeof(TaskSubmission), submissionId);
        submission.IsMarked = true;
        submission.TotalPoints = mark;
        await _context.SaveChangesAsync();
    }

    public async Task<TaskSubmission> GetByIdAsync(int id)
    {
        return await _context.TaskSubmissions
            .Include(t => t.Student)
            .Include(c => c.CourseTask)
            .SingleOrDefaultAsync(t => t.Id == id) ?? throw new ObjectNotFoundByIdException(typeof(TaskSubmission), id);
    }

    public async Task<TaskSubmission> GetByStudentAndCourse(int studentId, int courseTaskId)
    {
        return await _context.TaskSubmissions
                   .Include(t => t.Student)
                   .Include(t => t.CourseTask)
                   .SingleOrDefaultAsync(t => t.StudentId == studentId && t.CourseTaskId == courseTaskId) ??
               throw new InfrastructureExceptions(HttpStatusCode.NotFound,
                   $"There is not any submissions with TaskId = {courseTaskId} and StudentId = {studentId}");
    }
}