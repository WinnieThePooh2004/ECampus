using System.Net;

namespace UniversityTimetable.Shared.Exceptions.DomainExceptions
{
	public class Auth0RequestException : DomainException
	{
		public Auth0RequestException(HttpStatusCode statusCode, string message = "", object @object = null) : base(statusCode, message, @object)
		{
		}
	}
}
