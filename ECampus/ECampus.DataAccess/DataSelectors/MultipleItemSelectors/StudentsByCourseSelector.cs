using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Entities;
using ECampus.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class StudentsByCourseSelector : IParametersSelector<Student, StudentsByCourseParameters>
{
    public IQueryable<Student> SelectData(ApplicationDbContext context, StudentsByCourseParameters parameters)
    {
        return context.Courses
            .Include(course => course.Groups)!
            .ThenInclude(group => group.Students)
            .Where(course => course.Id == parameters.CourseId)
            .SelectMany(course => course.Groups!)
            .SelectMany(group => group.Students!);
    }
}