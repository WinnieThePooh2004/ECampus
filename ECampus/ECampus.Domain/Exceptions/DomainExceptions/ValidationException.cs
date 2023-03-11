using System.Net;
using ECampus.Domain.Validation;

namespace ECampus.Domain.Exceptions.DomainExceptions;

public class ValidationException : DomainException
{
    public ValidationException(Type type, ValidationResult errors) 
        : base(HttpStatusCode.BadRequest, $"{errors.Count()} errors occured while validating entity of type {type}", errors)
    {
    }
}