using System.Net;

namespace UniversityTimetable.Shared.Exceptions.DomainExceptions
{
	public class InvalidParameterValueExeption : DomainException
	{
		public InvalidParameterValueExeption(object @object, string parameterName, string message)
			:base(HttpStatusCode.BadRequest, $"{message}\nparameter '{parameterName}' has invalid value", @object)
		{
		}
	}
}
