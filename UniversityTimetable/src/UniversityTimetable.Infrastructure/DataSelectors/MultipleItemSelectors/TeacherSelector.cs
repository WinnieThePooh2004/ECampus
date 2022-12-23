using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors
{
    public class TeacherSelector : IMultipleItemSelector<Teacher, TeacherParameters>
    {
        public IQueryable<Teacher> SelectData(DbSet<Teacher> data, TeacherParameters parameters)
            => data.Search(g => g.LastName, parameters.SearchTerm)
            .Where(t => parameters.DepartmentId == 0 || t.DepartmentId == parameters.DepartmentId);
    }
}
