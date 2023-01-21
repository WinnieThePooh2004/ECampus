using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleCourseTaskSelector : IMultipleItemSelector<CourseTask, CourseTaskParameters>
{
    public IQueryable<CourseTask> SelectData(DbSet<CourseTask> data, CourseTaskParameters parameters)
    {
        return data.Where(c => c.CourseId == parameters.CourseId);
    }
}