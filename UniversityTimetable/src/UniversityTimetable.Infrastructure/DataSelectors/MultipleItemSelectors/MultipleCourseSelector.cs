using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleCourseSelector : IMultipleItemSelector<Course, CourseParameters>
{
    public IQueryable<Course> SelectData(DbSet<Course> data, CourseParameters parameters)
    {
        var result = data
            .Include(c => c.CourseGroups)
            .Include(c => c.CourseTeachers)
            .Search(c => c.Name, parameters.Name);

        if (parameters.GroupId != 0)
        {
            result = result.Where(c => c.CourseGroups!.Any(cg => cg.GroupId == parameters.GroupId));
        }

        if (parameters.TeacherId != 0)
        {
            result = result.Where(c => c.CourseTeachers!.Any(cg => cg.TeacherId == parameters.TeacherId));
        }

        return result;
    }
}