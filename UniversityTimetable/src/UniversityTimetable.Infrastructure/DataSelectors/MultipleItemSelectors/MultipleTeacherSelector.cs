using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleTeacherSelector : IMultipleItemSelector<Teacher, TeacherParameters>
{
    public IQueryable<Teacher> SelectData(DbSet<Teacher> data, TeacherParameters parameters)
    {
        var result = data.Search(s => s.FirstName, parameters.FirstName)
            .Search(s => s.LastName, parameters.LastName);
        if (parameters.DepartmentId != 0)
        {
            result = result.Where(s => s.DepartmentId == parameters.DepartmentId);
        }

        if (!parameters.UserIdCanBeNull)
        {
            result = result.Where(t => t.UserId == null);
        }

        return result;
    }
}