using System.Net;

namespace ECampus.Shared.Exceptions.DomainExceptions;

public class UnhandledDomainException : DomainException
{
    public UnhandledDomainException(Exception innerException, object? @object = null)
        : base(HttpStatusCode.InternalServerError,
            "Unhandled exception occured on domain level, view inner exception to see details", @object, innerException)
    {
    }
}