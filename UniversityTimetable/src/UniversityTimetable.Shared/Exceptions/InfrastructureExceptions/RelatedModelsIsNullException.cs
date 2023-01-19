using System.Net;

namespace UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;

public class RelatedModelsIsNullException : InfrastructureExceptions
{
    public RelatedModelsIsNullException(object failureObject, Type type)
        : base(HttpStatusCode.BadRequest,
            $"Please, send related models of object of type '{type}' as empty list instead of null",
            failureObject)
    {
    }
}