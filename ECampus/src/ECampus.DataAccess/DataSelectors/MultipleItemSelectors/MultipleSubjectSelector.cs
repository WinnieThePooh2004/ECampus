using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Extensions;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleSubjectSelector : IMultipleItemSelector<Subject, SubjectParameters>
{
    public IQueryable<Subject> SelectData(ApplicationDbContext context, SubjectParameters parameters)
        => context.Subjects.Search(s => s.Name, parameters.Name);
}