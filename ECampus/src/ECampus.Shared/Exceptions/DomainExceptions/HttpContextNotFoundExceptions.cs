using System.Net;

namespace ECampus.Shared.Exceptions.DomainExceptions;

public class HttpContextNotFoundExceptions : DomainException
{
    public HttpContextNotFoundExceptions() : base(HttpStatusCode.InternalServerError, "Http context not found")
    {
    }
}