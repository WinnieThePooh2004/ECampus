using System.Net;

namespace ECampus.Domain.Exceptions.InfrastructureExceptions;

public class ObjectNotFoundByIdException : InfrastructureExceptions
{
    public ObjectNotFoundByIdException(Type objectType, int id) : base(HttpStatusCode.NotFound,
        $"Object of type {objectType} with id={id} not found")
    {
    }
}