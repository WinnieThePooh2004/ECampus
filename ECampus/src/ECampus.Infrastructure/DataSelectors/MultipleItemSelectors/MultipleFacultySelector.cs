using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Extensions;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleFacultySelector : IMultipleItemSelector<Faculty, FacultyParameters>
{
    public IQueryable<Faculty> SelectData(DbSet<Faculty> data, FacultyParameters parameters)
        => data.Search(f => f.Name, parameters.Name);
}