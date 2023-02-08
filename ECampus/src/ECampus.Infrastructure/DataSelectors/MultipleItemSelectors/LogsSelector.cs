using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class LogsSelector : IMultipleItemSelector<Log, LogParameters>
{
    public IQueryable<Log> SelectData(ApplicationDbContext context, LogParameters parameters)
    {
         return context.Logs
             .Where(log => parameters.From <= log.TimeStamp &&
                           log.TimeStamp <= parameters.To &&
                           parameters.MinimalLevel <= log.Level &&
                           log.Level <= parameters.MaxLevel);
    }
}