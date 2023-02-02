using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Extensions;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleSubjectSelector : IMultipleItemSelector<Subject, SubjectParameters>
{
    public IQueryable<Subject> SelectData(ApplicationDbContext context, SubjectParameters parameters)
        => context.Subjects.Search(s => s.Name, parameters.Name);
}