using System.Net;

namespace UniversityTimetable.Shared.Exceptions.DomainExceptions
{
	public class DomainException : HttpResponseException
	{
		public DomainException(HttpStatusCode statusCode, string message = "", object @object = null) : base(statusCode, message, @object)
		{
		}
	}
}
