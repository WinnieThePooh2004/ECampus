namespace ECampus.Shared.Validation;

public readonly struct ValidationError
{
    public readonly string PropertyName;
    public readonly string ErrorMessage;
    
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