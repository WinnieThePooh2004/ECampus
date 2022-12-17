using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors
{
    public class TeacherSelector : IDataSelector<Teacher, TeacherParameters>
    {
        public IQueryable<Teacher> SelectData(DbSet<Teacher> data, TeacherParameters parameters)
            => data.Search(g => g.LastName, parameters.SearchTerm)
            .Where(t => parameters.DepartmentId == 0 || t.DepartmentId == parameters.DepartmentId);
    }
}
