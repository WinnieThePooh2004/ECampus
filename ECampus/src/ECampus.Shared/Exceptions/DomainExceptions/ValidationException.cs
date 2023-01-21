using System.Net;
using ECampus.Shared.Validation;

namespace ECampus.Shared.Exceptions.DomainExceptions;

public class ValidationException : DomainException
{
    public ValidationException(Type type, ValidationResult errors) 
        : base(HttpStatusCode.BadRequest, $"{errors.GetAllErrors().Count()} errors occured while validating entity of type {type}", errors)
    {
    }
}