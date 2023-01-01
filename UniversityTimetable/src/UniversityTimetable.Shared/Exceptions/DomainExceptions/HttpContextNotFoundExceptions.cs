using System.Net;

namespace UniversityTimetable.Shared.Exceptions.DomainExceptions;

public class HttpContextNotFoundExceptions : DomainException
{
    public HttpContextNotFoundExceptions() : base(HttpStatusCode.InternalServerError, "Http context not found")
    {
    }
}