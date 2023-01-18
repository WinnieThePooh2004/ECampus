namespace UniversityTimetable.Shared.Validation;

public sealed class ValidationError
{
    public string PropertyName { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;

    public ValidationError()
    {
        
    }

    public ValidationError(string propertyName, string errorMessage)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not ValidationError error)
        {
            return false;
        }

        return PropertyName == error.PropertyName && error.ErrorMessage == ErrorMessage;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PropertyName, ErrorMessage);
    }
}