using System.Net;

namespace UniversityTimetable.Shared.Exceptions.DomainExceptions
{
	public class DomainException : HttpResponseExeption
	{
		public DomainException(HttpStatusCode statusCode, string message = "", object @object = null) : base(statusCode, message, @object)
		{
		}
	}
}
