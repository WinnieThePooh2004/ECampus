using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Extensions;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleAuditorySelector : IParametersSelector<Auditory, AuditoryParameters>
{
    public IQueryable<Auditory> SelectData(ApplicationDbContext context, AuditoryParameters parameters)
        => context.Auditories.Search(a => a.Name, parameters.AuditoryName)
            .Search(a => a.Building, parameters.BuildingName);
}