using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Entities;
using ECampus.Domain.Extensions;
using ECampus.Domain.Requests.Auditory;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleAuditorySelector : IParametersSelector<Auditory, AuditoryParameters>
{
    public IQueryable<Auditory> SelectData(ApplicationDbContext context, AuditoryParameters parameters)
        => context.Auditories.Search(a => a.Name, parameters.AuditoryName)
            .Search(a => a.Building, parameters.BuildingName);
}