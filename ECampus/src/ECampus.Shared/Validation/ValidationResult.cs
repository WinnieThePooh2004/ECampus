﻿using ECampus.Shared.Extensions;
using Newtonsoft.Json;

namespace ECampus.Shared.Validation;

public class ValidationResult
{
    [JsonProperty]
    private Dictionary<string, List<string>> Errors { get; } = new();

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

    public void AddError(ValidationError error)
    {
        if (!Errors.ContainsKey(error.PropertyName))
        {
            Errors.Add(error.PropertyName, new List<string> { error.ErrorMessage });
            return;
        }

        Errors[error.PropertyName].Add(error.ErrorMessage);
    }

    public IEnumerable<ValidationError> GetAllErrors() =>
        Errors.SelectMany(e =>
            e.Value.Select(message => new ValidationError(e.Key, message)));

    public bool IsValid => !Errors.Any();
}