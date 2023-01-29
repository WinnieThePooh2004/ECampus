using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataCreateServices;

public class StudentCreateService : IDataCreateService<Student>
{
    private readonly IDataCreateService<Student> _baseCreate;

    public StudentCreateService(IDataCreateService<Student> baseCreate)
    {
        _baseCreate = baseCreate;
    }

    public async Task<Student> CreateAsync(Student model, DbContext context)
    {
        model.Submissions = await context.Set<Group>()
            .AsNoTracking()
            .Include(group => group.Courses!)
            .ThenInclude(course => course.Tasks)
            .Where(group => group.Id == model.GroupId)
            .SelectMany(group => group.Courses!)
            .SelectMany(course => course.Tasks!)
            .Select(task => new TaskSubmission { CourseTaskId = task.Id })
            .ToListAsync();

        return await _baseCreate.CreateAsync(model, context);
    }
}