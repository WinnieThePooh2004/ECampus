namespace ECampus.Domain.Metadata;

[AttributeUsage(AttributeTargets.Class)]
public class ValidationAttribute : Attribute
{
    public bool DecorateServices { get; }
    public bool ValidateUpdate { get; }
    public bool ValidateCreate { get; }

    public ValidationAttribute(bool validateUpdate = true, bool validateCreate = true, bool decorateServices = true)
    {
        DecorateServices = decorateServices;
        ValidateUpdate = validateUpdate;
        ValidateCreate = validateCreate;
    }
}