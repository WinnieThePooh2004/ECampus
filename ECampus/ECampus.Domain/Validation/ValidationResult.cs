using System.Collections;
using Newtonsoft.Json;

namespace ECampus.Domain.Validation;

public class ValidationResult : IEnumerable<ValidationError>
{
    [JsonProperty] private Dictionary<string, List<string>> Errors { get; } = new();

    public ValidationResult()
    {
    }

    public ValidationResult(IEnumerable<ValidationError> errors)
    {
        this.AddRange(errors);
    }

    public ValidationResult(params ValidationError[] errors)
        : this((IEnumerable<ValidationError>)errors)
    {
    }

    public ValidationResult(string propertyName, string message)
    {
        AddError(new ValidationError(propertyName, message));
    }

    public void AddError(ValidationError error)
    {
        if (!Errors.ContainsKey(error.PropertyName))
        {
            Errors.Add(error.PropertyName, new List<string> { error.ErrorMessage });
            return;
        }

        Errors[error.PropertyName].Add(error.ErrorMessage);
    }

    public bool IsValid => !Errors.Any();

    public IEnumerator<ValidationError> GetEnumerator()
        => Errors.SelectMany(e =>
            e.Value.Select(message => new ValidationError(e.Key, message))).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}