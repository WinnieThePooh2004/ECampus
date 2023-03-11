using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Extensions;
using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleFacultySelector : IParametersSelector<Faculty, FacultyParameters>
{
    public IQueryable<Faculty> SelectData(ApplicationDbContext context, FacultyParameters parameters)
        => context.Faculties.Search(f => f.Name, parameters.Name);
}