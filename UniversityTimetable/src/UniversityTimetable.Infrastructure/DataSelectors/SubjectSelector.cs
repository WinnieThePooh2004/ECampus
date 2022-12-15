using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors
{
    public class SubjectSelector : IDataSelector<Subject, SubjectParameters>
    {
        public IQueryable<Subject> SelectData(DbSet<Subject> data, SubjectParameters parameters)
            => data.Search(s => s.Name, parameters.SearchTerm)
            .Where(s => parameters.TeacherId == 0 || s.TeacherIds.Any(t => t.TeacherId == parameters.TeacherId));
    }
}
