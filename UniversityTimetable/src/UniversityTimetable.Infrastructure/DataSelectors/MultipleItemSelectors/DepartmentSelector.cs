using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors
{
    public class DepartmentSelector : IMultipleItemSelector<Department, DepartmentParameters>
    {
        public IQueryable<Department> SelectData(DbSet<Department> data, DepartmentParameters parameters)
            => data.Search(d => d.Name, parameters.SearchTerm)
                .Where(d => parameters.FacultacyId == 0 || d.FacultyId == parameters.FacultacyId);
    }
}
