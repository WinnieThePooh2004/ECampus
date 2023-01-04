using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleFacultySelector : IMultipleItemSelector<Faculty, FacultyParameters>
{
    public IQueryable<Faculty> SelectData(DbSet<Faculty> data, FacultyParameters parameters)
        => data.Search(f => f.Name, parameters.SearchTerm);
}