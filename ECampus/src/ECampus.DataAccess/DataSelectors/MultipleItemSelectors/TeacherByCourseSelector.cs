using ECampus.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class TeacherByCourseSelector : IMultipleItemSelector<Teacher, TeacherByCourseParameters>
{
    public IQueryable<Teacher> SelectData(ApplicationDbContext context, TeacherByCourseParameters parameters)
    {
        return context.Courses
            .Include(course => course.Teachers)
            .Where(course => course.Id == parameters.CourseId)
            .SelectMany(course => course.Teachers!);
    }
}