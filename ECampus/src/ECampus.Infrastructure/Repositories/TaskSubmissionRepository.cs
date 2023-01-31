using System.Net;
using ECampus.Core.Metadata;
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

    public async Task<TaskSubmission> UpdateContentAsync(int submissionId, string content)
    {
        var submission = await _context.TaskSubmissions
            .Include(t => t.Student)
            .Include(t => t.CourseTask)
            .SingleOrDefaultAsync(t => t.Id == submissionId) ?? throw new ObjectNotFoundByIdException(
            typeof(TaskSubmission), submissionId);

        submission.SubmissionContent = content;
        await _context.SaveChangesAsync();
        return submission;
    }

    public async Task<TaskSubmission> UpdateMarkAsync(int submissionId, int mark)
    {
        var submission = await _context.TaskSubmissions
            .Include(t => t.Student)
            .Include(t => t.CourseTask)
            .SingleOrDefaultAsync(t => t.Id == submissionId) ?? throw new ObjectNotFoundByIdException(
            typeof(TaskSubmission), submissionId);
        
        submission.IsMarked = true;
        submission.TotalPoints = mark;
        await _context.SaveChangesAsync();
        return submission;
    }

    public async Task<TaskSubmission> GetByIdAsync(int id)
    {
        return await _context.TaskSubmissions
            .Include(t => t.Student)
            .Include(c => c.CourseTask)
            .SingleOrDefaultAsync(t => t.Id == id) ?? throw new ObjectNotFoundByIdException(typeof(TaskSubmission), id);
    }

    public async Task<TaskSubmission> GetByStudentAndCourseAsync(int studentId, int courseTaskId)
    {
        return await _context.TaskSubmissions
                   .Include(t => t.Student)
                   .Include(t => t.CourseTask)
                   .SingleOrDefaultAsync(t => t.StudentId == studentId && t.CourseTaskId == courseTaskId) ??
               throw new InfrastructureExceptions(HttpStatusCode.NotFound,
                   $"There is not any submissions with TaskId = {courseTaskId} and StudentId = {studentId}");
    }
}