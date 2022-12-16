using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace UniversityTimetable.Shared.Exceptions.DomainExceptions
{
    public class ClassValidationException : DomainException
    {
        public ClassValidationException([NotNull] Dictionary<string, string> errors) 
            : base(HttpStatusCode.BadRequest, "One or more validation errors occured", errors)
        {
        }
    }
}
