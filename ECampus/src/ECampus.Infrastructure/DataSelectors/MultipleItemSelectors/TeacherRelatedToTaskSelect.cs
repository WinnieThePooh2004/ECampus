using ECampus.Contracts.DataSelectParameters;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class TeacherRelatedToTaskSelect : IMultipleItemSelector<Teacher, TeacherRelatedToTaskParameters>
{
    public IQueryable<Teacher> SelectData(ApplicationDbContext context, TeacherRelatedToTaskParameters parameters)
    {
        return context.Teachers
            .Include(teacher => teacher.Courses)!
            .ThenInclude(course => course.Tasks!.Where(task => task.Id == parameters.CourseTaskId));
    }
}