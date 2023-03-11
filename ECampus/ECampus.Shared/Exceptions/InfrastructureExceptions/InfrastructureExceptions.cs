using System.Net;

namespace ECampus.Shared.Exceptions.InfrastructureExceptions;

public class InfrastructureExceptions : HttpResponseException
{
    public InfrastructureExceptions(HttpStatusCode statusCode, string message = "", object? @object = null,
        Exception? innerException = null)
        : base(statusCode, message, @object, innerException)
    {
    }
}