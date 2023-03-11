using ECampus.Shared.Validation;

namespace ECampus.FrontEnd.Requests.Interfaces.Validation;

public interface IValidationRequests<in T>
{
    Task<ValidationResult> ValidateAsync(T data);
}