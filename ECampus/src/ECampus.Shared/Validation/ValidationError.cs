namespace ECampus.Shared.Validation;

public record struct ValidationError(string PropertyName, string ErrorMessage);