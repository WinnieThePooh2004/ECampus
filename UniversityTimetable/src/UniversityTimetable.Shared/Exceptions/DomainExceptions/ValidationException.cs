using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace UniversityTimetable.Shared.Exceptions.DomainExceptions
{
    public class ValidationException : DomainException
    {
        public ValidationException(Type type, [NotNull] List<KeyValuePair<string, string>> errors) 
            : base(HttpStatusCode.BadRequest, $"{errors.Count} errors occured while validating entity of type {type}", errors)
        {
        }
    }
}
