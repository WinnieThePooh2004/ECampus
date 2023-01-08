namespace UniversityTimetable.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ValidationAttribute : Attribute
{
    public ValidationTypes ValidationType { get; }

    public bool RequireService { get; }

    public bool UseUniversalValidator { get; }

    public ValidationAttribute(ValidationTypes type = ValidationTypes.CreateAndUpdate, bool requireService = true,
        bool useUniversalValidator = true)
    {
        ValidationType = type;
        UseUniversalValidator = useUniversalValidator;
        RequireService = requireService;
    }
}

public enum ValidationTypes
{
    None,
    CreateOnly,
    UpdateOnly,
    CreateAndUpdate
}