using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Extensions;
using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleAuditorySelector : IParametersSelector<Auditory, AuditoryParameters>
{
    public IQueryable<Auditory> SelectData(ApplicationDbContext context, AuditoryParameters parameters)
        => context.Auditories.Search(a => a.Name, parameters.AuditoryName)
            .Search(a => a.Building, parameters.BuildingName);
}