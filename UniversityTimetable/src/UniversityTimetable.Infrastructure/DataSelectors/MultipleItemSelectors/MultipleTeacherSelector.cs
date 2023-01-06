using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleTeacherSelector : IMultipleItemSelector<Teacher, TeacherParameters>
{
    public IQueryable<Teacher> SelectData(DbSet<Teacher> data, TeacherParameters parameters)
        => data.Search(g => g.LastName, parameters.SearchTerm)
            .Search(t => t.FirstName, parameters.FirstName)
            .Where(t => parameters.DepartmentId == 0 || t.DepartmentId == parameters.DepartmentId);
}