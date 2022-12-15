using System.Net;

namespace UniversityTimetable.Shared.Exceptions.DomainExceptions
{
    public class ExistingObjectAddException : DomainException
    {
        public ExistingObjectAddException(object @object = null) 
            : base(HttpStatusCode.BadRequest, "Object`s sent to post method must be 0", @object)
        {
        }
    }
}
