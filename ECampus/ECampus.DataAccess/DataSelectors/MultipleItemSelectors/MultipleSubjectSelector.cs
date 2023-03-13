using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Entities;
using ECampus.Domain.Extensions;
using ECampus.Domain.Requests.Subject;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleSubjectSelector : IParametersSelector<Subject, SubjectParameters>
{
    public IQueryable<Subject> SelectData(ApplicationDbContext context, SubjectParameters parameters)
        => context.Subjects.Search(s => s.Name, parameters.Name);
}