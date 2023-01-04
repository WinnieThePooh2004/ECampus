using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleSubjectSelector : IMultipleItemSelector<Subject, SubjectParameters>
{
    public IQueryable<Subject> SelectData(DbSet<Subject> data, SubjectParameters parameters)
        => data.Search(s => s.Name, parameters.SearchTerm);
}