using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Extensions;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleFacultySelector : IParametersSelector<Faculty, FacultyParameters>
{
    public IQueryable<Faculty> SelectData(ApplicationDbContext context, FacultyParameters parameters)
        => context.Faculties.Search(f => f.Name, parameters.Name);
}