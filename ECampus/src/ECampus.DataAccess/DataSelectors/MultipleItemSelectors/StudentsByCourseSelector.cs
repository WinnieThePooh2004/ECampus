using ECampus.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

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