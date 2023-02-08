using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class LogsSelector : IMultipleItemSelector<Log, LogParameters>
{
    public IQueryable<Log> SelectData(ApplicationDbContext context, LogParameters parameters)
    {
        return context.Logs
            .Where(log => log.TimeStamp >= parameters.From &&
                          log.TimeStamp <= parameters.To &&
                          (parameters.ExceptionRequired == false || log.Exception != null) &&
                          log.Level <= parameters.MaxLevel &&
                          log.Level >= parameters.MinimalLevel);
    }
}