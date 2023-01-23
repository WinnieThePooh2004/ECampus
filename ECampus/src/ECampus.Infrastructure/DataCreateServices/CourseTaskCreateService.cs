using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataCreateServices;

public class CourseTaskCreateService : IDataCreateService<CourseTask>
{
    private readonly IDataCreateService<CourseTask> _baseCreate;

    public CourseTaskCreateService(IDataCreateService<CourseTask> baseCreate)
    {
        _baseCreate = baseCreate;
    }

    public async Task<CourseTask> CreateAsync(CourseTask model, DbContext context)
    {
        model.Submissions = await context.Set<Course>()
            .Include(course => course.Groups)!
            .ThenInclude(group => group.Students)
            .Where(course => course.Id == model.CourseId)
            .SelectMany(course => course.Groups!)
            .SelectMany(group => group.Students!)
            .Select(student => new TaskSubmission { StudentId = student.Id })
            .ToListAsync();

        return await _baseCreate.CreateAsync(model, context);
    }
}