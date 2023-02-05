using ECampus.Contracts.DataSelectParameters;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class StudentsByCourseSelector : IMultipleItemSelector<Student, StudentsByCourseParameters>
{
    public IQueryable<Student> SelectData(ApplicationDbContext context, StudentsByCourseParameters parameters)
    {
        return context.Courses
            .Include(course => course.Groups)!
            .ThenInclude(group => group.Students)
            .SelectMany(course => course.Groups!)
            .SelectMany(group => group.Students!);
    }
}