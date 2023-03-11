using System.Net;

namespace ECampus.Domain.Exceptions.DomainExceptions;

public class HttpContextNotFoundExceptions : DomainException
{
    public HttpContextNotFoundExceptions() : base(HttpStatusCode.InternalServerError, "Http context not found")
    {
    }
}