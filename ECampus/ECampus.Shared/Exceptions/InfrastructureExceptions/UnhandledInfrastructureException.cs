using System.Net;

namespace ECampus.Shared.Exceptions.InfrastructureExceptions;

public class UnhandledInfrastructureException : InfrastructureExceptions
{
    public UnhandledInfrastructureException(Exception innerException, object? @object = null)
        : base(HttpStatusCode.InternalServerError,
            "Unhandled exception occured on data access level, view inner exception to see details", @object,
            innerException)
    {
    }
}