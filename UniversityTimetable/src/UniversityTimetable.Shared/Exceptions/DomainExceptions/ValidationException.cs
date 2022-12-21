using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace UniversityTimetable.Shared.Exceptions.DomainExceptions
{
    public class ValidationException : DomainException
    {
        public ValidationException(Type type, [NotNull] Dictionary<string, string> errors) 
            : base(HttpStatusCode.BadRequest, $"Cannot validate object of type {type}", errors)
        {
        }
    }
}
