using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors
{
    public class FacultySelector : IDataSelector<Faculty, FacultyParameters>
    {
        public IQueryable<Faculty> SelectData(DbSet<Faculty> data, FacultyParameters parameters)
            => data.Search(f => f.Name, parameters.SearchTerm);
    }
}
