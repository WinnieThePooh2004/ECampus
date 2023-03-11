using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Models;
using ECampus.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class TeacherRelatedToTaskSelect : IParametersSelector<Teacher, TeacherRelatedToTaskParameters>
{
    public IQueryable<Teacher> SelectData(ApplicationDbContext context, TeacherRelatedToTaskParameters parameters)
    {
        return context.Teachers
            .Include(teacher => teacher.Courses)!
            .ThenInclude(course => course.Tasks!.Where(task => task.Id == parameters.CourseTaskId));
    }
}