using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class LogsSelector : IParametersSelector<Log, LogParameters>
{
    public IQueryable<Log> SelectData(ApplicationDbContext context, LogParameters parameters)
    {
         return context.Set<Log>()
             .Where(log => parameters.From <= log.TimeStamp &&
                           log.TimeStamp <= parameters.To);
    }
}