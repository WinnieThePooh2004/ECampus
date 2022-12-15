using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Exceptions;

namespace UniversityTimetable.Shared.Extentions
{
    public static class ILoggerExtentions
    {
        public static void LogAndThrowException(this ILogger logger, HttpResponseException exception)
        {
            logger.LogMessageAndThrowException(exception, exception.Message);
        }

        public static void LogMessageAndThrowException(this ILogger logger, HttpResponseException exception, string message)
        {
            logger.LogError(exception, "{message}", message);
            throw exception;
        }
    }
}
