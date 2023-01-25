using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataAccessFacades;

[Inject(typeof(ICourseTaskMessageDataAccess))]
public class CourseTaskMessageDataAccess : ICourseTaskMessageDataAccess
{
    private readonly DbContext _context;

    public CourseTaskMessageDataAccess(DbContext context)
    {
        _context = context;
    }

    public async Task<(string courseName, List<string> studentEmails)> LoadDataForSendMessage(int courseId)
    {
        var course = await _context.Set<Course>()
                         .AsNoTracking()
                         .Include(course => course.Groups)!
                         .ThenInclude(group => group.Students)
                         .SingleOrDefaultAsync(course => course.Id == courseId) ??
                     throw new ObjectNotFoundByIdException(typeof(Course), courseId);

        var emails = course.Groups!
            .SelectMany(group => group.Students!)
            .Where(student => student.UserEmail is not null)
            .Select(student => student.UserEmail)
            .ToList();

        return (course.Name, emails)!;
    }
}