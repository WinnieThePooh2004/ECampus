using System.Net;

namespace ECampus.Shared.Exceptions.DomainExceptions;

public class DomainException : HttpResponseException
{
    public DomainException(HttpStatusCode statusCode, string message = "", object? @object = null, Exception? innerException = null) 
        : base(statusCode, message, @object, innerException)
    {
    }
}