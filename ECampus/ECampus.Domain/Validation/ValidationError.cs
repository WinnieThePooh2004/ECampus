namespace ECampus.Domain.Validation;

public record struct ValidationError(string PropertyName, string ErrorMessage);