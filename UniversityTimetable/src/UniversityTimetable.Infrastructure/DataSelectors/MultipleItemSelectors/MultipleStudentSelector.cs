using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleStudentSelector : IMultipleItemSelector<Student, StudentParameters>
{
    public IQueryable<Student> SelectData(DbSet<Student> data, StudentParameters parameters)
    {
        var result = data.Search(s => s.FirstName, parameters.FirstName)
            .Search(s => s.LastName, parameters.LastName);
        if (parameters.GroupId != 0)
        {
            result = result.Where(s => s.GroupId == parameters.GroupId);
        }

        if (!parameters.UserIdCanBeNull)
        {
            result = result.Where(t => t.UserId == null);
        }

        return result;
    }
}